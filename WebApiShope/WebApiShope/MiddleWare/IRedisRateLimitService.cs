namespace WebApiShope.MiddleWare
{
    public interface IRedisRateLimitService
    {
        /// <summary>
        /// Atomically increments the counter for <paramref name="key"/> and sets its TTL
        /// to <paramref name="windowSeconds"/> on the first increment (fixed window boundary).
        /// </summary>
        /// <returns>
        /// A tuple containing:
        /// - Count: the new counter value after this request.
        /// - TtlSeconds: seconds remaining until the current window expires.
        /// </returns>
        Task<(long Count, long TtlSeconds)> IncrementAsync(string key, int windowSeconds);
    }
}