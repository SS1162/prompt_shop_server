using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record AddBasicSiteDTO
    {
        [Required]
        public string SiteName { get; set; }

        public long? UserDescreption { get; set; }
        public long? SiteTypeID { get; set; }

        public long PlatformID { get; set; }

    }
    
}
