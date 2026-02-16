using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record FullOrderDTO
      (
         [Required]
         long OrderID, 
        [Required]
         long UserID,
         [Required]
         float OrderSum,

        [Required]
         long BasicID,
         [Required]
         long StatusId,

        [Required]
         List<AddToCartDTO> Products
        );
}
