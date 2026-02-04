using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record OrderItemDTO
    (

        int OrderItemID ,

        string UserDescription ,

        string PlatformName ,

        int ProductsName ,

        float Price ,

        string CategoryName ,

        string ImgUrl ,

        string CategoryDescreption ,

        int orderID,

        int UserDescriptionID

    );
}
