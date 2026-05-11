using StackExchange.Redis;

namespace WebApiShope.MiddleWare
{
   
    public class RedisRateLimitService : IRedisRateLimitService
    {
        private readonly IConnectionMultiplexer _redis;

        // Lua script executed atomically on the Redis server:
        //   1. INCR key              → create-or-increment the counter
        //   2. EXPIRE key window     → set TTL only on the first call so the window
        //                              boundary stays fixed rather than sliding
        //   3. TTL key               → return remaining seconds for the X-Rate-Limit-Reset header
        // Using EVAL ensures INCR + EXPIRE are a single atomic unit, eliminating
        // the race condition where two concurrent requests both see count == 0.
        private const string LuaScript = """
            local current = redis.call('INCR', KEYS[1])
            if current == 1 then
                redis.call('EXPIRE', KEYS[1], ARGV[1])
            end
            local ttl = redis.call('TTL', KEYS[1])
            return {current, ttl}
            """;

        public RedisRateLimitService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<(long Count, long TtlSeconds)> IncrementAsync(string key, int windowSeconds)
        {
            var db = _redis.GetDatabase();

            // ScriptEvaluateAsync sends EVAL; Redis caches the compiled script by its SHA
            // automatically, so subsequent calls use EVALSHA with no extra round-trip.
            var result = (RedisResult[]?)await db.ScriptEvaluateAsync(
                LuaScript,
                keys: [key],
                values: [(RedisValue)windowSeconds]
            );

            if (result is null || result.Length < 2)
                return (1, windowSeconds); // safe fallback: treat as first request

            long count  = (long)result[0];
            long ttl    = (long)result[1];

            // TTL is -1 when EXPIRE has not been applied yet (should not happen with the
            // Lua script, but guard defensively at this boundary with external Redis).
            if (ttl < 0) ttl = windowSeconds;

            return (count, ttl);
        }
    }
}
