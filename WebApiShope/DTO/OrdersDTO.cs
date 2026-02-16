using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record OrdersDTO
    (
        [Required]
         long UserID,
         [Required]
         float OrderSum,

        [Required]
         long BasicID,

        [Required]
         List<AddToCartDTO> Products
        );
}
