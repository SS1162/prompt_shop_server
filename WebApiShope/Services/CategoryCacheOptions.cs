namespace Services
{
    public class CategoryCacheOptions
    {
        public const string SectionName = "CategoryCache";

        // Default TTL if configuration is missing.
        public int TtlSeconds { get; set; } = 120;
    }
}
