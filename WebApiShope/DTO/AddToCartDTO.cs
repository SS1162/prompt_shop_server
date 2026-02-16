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
        long UserID ,
        [Required]
        long ProductsID,
        string UserDescription ,
        [Required]
        long PlatformsID,
        long UserDescriptionID


    );
}
