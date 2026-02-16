using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record AddBasicSiteDTO
    (
        [Required]
     string SiteName,
    string UserDescreption,
      [Required]
     long SiteTypeID,
        [Required]
     long PlatformID
);
    
}
