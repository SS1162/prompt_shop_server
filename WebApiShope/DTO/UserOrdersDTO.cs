using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public  record UserOrdersDTO
    (
         long UserID,

         long OrderID ,
         DateTime OrderDate ,
         float OrderSum ,
         string SiteName ,
         string SiteTypeName ,
         string SiteTypeDescreption ,
         string StatusName ,
         string PlatformName ,
         int LenOrderItems ,
         string ReviewID ,
         float Stars ,
         string ReviewImg 
    );
}
