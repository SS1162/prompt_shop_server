using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Net;
using System.Security.Claims;

namespace WebApiShope.MiddleWare
{
    /// <summary>
    /// Fixed Window Rate Limiting middleware backed by Redis.
    ///
    /// Identification strategy (dual-key):
    ///   • Authenticated users  → checked against TWO independent keys:
    ///       ratelimit:user:{username}:{windowBucket}
    ///       ratelimit:ip:{ip}:{windowBucket}
    ///     A request is blocked when EITHER counter exceeds AuthenticatedLimit.
    ///   • Anonymous users      → checked against ONE key:
    ///       ratelimit:ip:{ip}:{windowBucket}
    ///     A request is blocked when the counter exceeds AnonymousLimit.
    ///
    /// The middleware is registered early in the pipeline so that blocked requests
    /// never reach authentication, routing, or controller logic.
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitOptions _options;
        private readonly IRedisRateLimitService _rateLimitService;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        public RateLimitingMiddleware(
            RequestDelegate next,
            IOptions<RateLimitOptions> options,
            IRedisRateLimitService rateLimitService,
            ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _rateLimitService = rateLimitService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = GetClientIp(context);
            bool isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

            int limit;
            string primaryKey;
            string? secondaryKey = null; // IP key used only for authenticated users

            if (isAuthenticated)
            {
                // Prefer Name claim; fall back to Email or NameIdentifier (sub) from JWT
                var userName = context.User.Identity!.Name
                    ?? context.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? "unknown";

                limit        = _options.AuthenticatedLimit;
                primaryKey   = BuildKey("user", SanitizeSegment(userName), _options.WindowSeconds);
                secondaryKey = BuildKey("ip",   SanitizeSegment(ip),       _options.WindowSeconds);
            }
            else
            {
                limit      = _options.AnonymousLimit;
                primaryKey = BuildKey("ip", SanitizeSegment(ip), _options.WindowSeconds);
            }

            try
            {
                // ── Increment counters ────────────────────────────────────────────────
                // Both async calls are started together so they run concurrently, each
                // going to Redis without waiting for the other to finish first.
                var primaryTask   = _rateLimitService.IncrementAsync(primaryKey,   _options.WindowSeconds);
                var secondaryTask = secondaryKey is not null
                    ? _rateLimitService.IncrementAsync(secondaryKey, _options.WindowSeconds)
                    : Task.FromResult((Count: 0L, TtlSeconds: (long)_options.WindowSeconds));

                var (primaryCount, primaryTtl)     = await primaryTask;
                var (secondaryCount, secondaryTtl) = await secondaryTask;

                // ── Determine the "tightest" counter ─────────────────────────────────
                // Block if either bucket is over the limit; show headers for whichever
                // counter is closest to the ceiling so the client sees the binding constraint.
                bool primaryBlocked   = primaryCount   > limit;
                bool secondaryBlocked = secondaryCount > limit;
                bool blocked          = primaryBlocked || secondaryBlocked;

                // The binding counter is whichever is higher relative to the limit.
                long effectiveCount = Math.Max(primaryCount, secondaryCount);
                long effectiveTtl   = primaryCount >= secondaryCount ? primaryTtl : secondaryTtl;

                long remaining = Math.Max(0, limit - effectiveCount);
                long resetUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + effectiveTtl;

                // ── Standard rate-limit headers (always present) ──────────────────────
                context.Response.Headers["X-Rate-Limit-Limit"]     = limit.ToString();
                context.Response.Headers["X-Rate-Limit-Remaining"] = remaining.ToString();
                context.Response.Headers["X-Rate-Limit-Reset"]     = resetUnix.ToString();

                if (blocked)
                {
                    string blockingKey = primaryBlocked ? primaryKey : secondaryKey!;
                    _logger.LogWarning(
                        "Rate limit exceeded. Key={Key} Count={Count} Limit={Limit} IP={IP}",
                        blockingKey, effectiveCount, limit, ip);

                    context.Response.StatusCode               = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"]   = effectiveTtl.ToString();
                    context.Response.ContentType              = "text/plain";
                    await context.Response.WriteAsync(
                        "Too Many Requests. Please slow down and try again later.");
                    return;
                }
            }
            catch (RedisException ex)
            {
                // Fail open: if Redis is unavailable, let the request through rather
                // than making the whole API unavailable. Log it so ops can react.
                _logger.LogError(ex,
                    "Redis rate-limit check failed for IP {IP}. Allowing request.", ip);
            }

            await _next(context);
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Builds a Redis key that encodes the identifier type, value, and the current
        /// fixed-window bucket so that the counter automatically resets each window.
        ///
        /// Format: ratelimit:{type}:{identifier}:{windowBucket}
        /// windowBucket = Unix seconds ÷ windowSeconds  (integer division)
        /// </summary>
        private static string BuildKey(string type, string identifier, int windowSeconds)
        {
            long windowBucket = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / windowSeconds;
            return $"ratelimit:{type}:{identifier}:{windowBucket}";
        }

        /// <summary>
        /// Extracts the real client IP, honouring the X-Forwarded-For header set by
        /// reverse proxies and load balancers. Always takes the first (leftmost) address
        /// in the chain, which represents the original client.
        /// </summary>
        private static string GetClientIp(HttpContext context)
        {
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded))
            {
                var first = forwarded.Split(',')[0].Trim();
                if (IPAddress.TryParse(first, out _))
                    return first;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        /// <summary>
        /// Replaces characters that would break the key structure (':' is the separator;
        /// spaces and other whitespace can cause confusion).
        /// </summary>
        private static string SanitizeSegment(string value)
            => value.Replace(':', '_').Replace(' ', '_');
    }

    // Extension method so Program.cs reads as: app.UseRateLimiting();
    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
          return builder.UseMiddleware<RateLimitingMiddleware>();  
        }
            
            
    }
}
