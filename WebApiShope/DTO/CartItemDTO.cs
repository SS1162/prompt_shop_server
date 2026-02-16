using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CartItemDTO
    (
         long CartID ,

         string ProductsName ,

         float Price, 

         string CategoryName ,

         string ImgUrl ,

         string CategoryDescreption ,

         int Valid ,

         string UserDescription ,

         string PlatformName ,

         long productID

    );
}
