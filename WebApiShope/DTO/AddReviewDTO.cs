using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record AddReviewDTO(
        [Required]
           long OrderId,
        [Required]
           int Score,
           string Note,
           string ReviewImageUrl
       );
}
