namespace WebApiShope.MiddleWare
{
    /// <summary>
    /// Configuration for the Fixed Window Rate Limiting middleware.
    /// Bind this to the "RateLimiting" section in appsettings.json.
    /// </summary>
    public class RateLimitOptions
    {
        public const string SectionName = "RateLimiting";

        /// <summary>
        /// Maximum requests per window for anonymous users (identified by IP address only).
        /// </summary>
        public int AnonymousLimit { get; set; } = 60;

        /// <summary>
        /// Maximum requests per window for authenticated users.
        /// Applied independently to both the per-user key and the per-IP key.
        /// A request is blocked when EITHER key exceeds this limit.
        /// </summary>
        public int AuthenticatedLimit { get; set; } = 200;

        /// <summary>
        /// Fixed window duration in seconds.
        /// </summary>
        public int WindowSeconds { get; set; } = 60;
    }
}
