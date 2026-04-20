using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record SiteTypeDTO
    (
         long? SiteTypeID ,

        string SiteTypeName ,

        string SiteTypeDescreption ,

        float Price 
       

    );
}
