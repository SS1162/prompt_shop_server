using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record UpdateBasicSiteDTO
    {
        [Required]
        public long BasicSiteID { get; init; }

        [Required]
        public string SiteName { get; init; } = string.Empty;

        public long? UserDescreption { get; init; }

        public long? SiteTypeID { get; init; }

        [Required]
        public long PlatformID { get; init; }
    }
}
