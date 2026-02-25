using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace DTO
{
     public record AddCategoryDTO
    (
        [Required]
         long MainCategoryID ,
        [Required]

         string CategoryName,
        [Required]

         IFormFile ImgUrl,
        [Required]
         string CategoryDescreption
    );
}
