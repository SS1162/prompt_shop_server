using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record UserDTO
    (
        [Required]
         string UserID ,

        [EmailAddress]
        [Required]
         string UserName ,
 
         string FirstName ,


         string LastName ,

        [Phone]
         string Phone ,

        long BasicID

    );
}
