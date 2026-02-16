using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record UpdateProductDTO
    (
         long ProductID ,
         long CategoryID ,
         string ProductsName ,

         float Price

    );
}
