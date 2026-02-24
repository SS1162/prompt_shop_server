using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{

    public record BasicSiteDTO
    {
        public long BasicSiteID { get; init; }
        public string SiteName { get; init; } = string.Empty;
        public string? UserDescreption { get; init; }
        public long? GeminiPromptId { get; init; }
        public string PlatformName { get; init; } = string.Empty;
        public string? SiteTypeName { get; init; }
        public long PlatformID { get; init; }
        public long? SiteTypeID { get; init; }
        public string? SiteTypeDescreption { get; init; }
    }
}
