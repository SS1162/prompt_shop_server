using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{

    public record BasicSiteDTO
    (
        long BasicSiteID ,
        string SiteName ,
        string UserDescreption ,
        string PlatformName ,

        string SiteTypeName ,

        long PlatformID ,

        long SiteTypeID ,
         string SiteTypeDescreption 
    );
}
