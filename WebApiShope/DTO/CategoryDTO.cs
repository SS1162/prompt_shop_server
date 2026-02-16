using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CategoryDTO
    (
        [Required]
         long CategoryID ,

        [Required]
         long MainCategoryID ,

        [Required]
         string CategoryName ,
        [Required]
         string ImgUrl ,
        [Required]
         string CategoryDescreption 
    );
}
