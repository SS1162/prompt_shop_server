using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CategoryToUpdateDTO
    (
        [Required]
         long CategoryID,

        [Required]
         long MainCategoryID,

        [Required]
         string CategoryName,
        [Required]
         IFormFile ImgUrl,
        [Required]
         string CategoryDescreption
    );
}
