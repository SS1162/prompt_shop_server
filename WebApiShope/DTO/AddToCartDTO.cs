using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public  record AddToCartDTO
    (   [Required]
        int UserID ,
        [Required]
        int ProductsID,
        string UserDescription ,
        [Required]
        int PlatformsID,
        int UserDescriptionID


    );
}
