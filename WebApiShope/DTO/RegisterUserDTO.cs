using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public record RegisterUserDTO
    (
        [EmailAddress]
        [Required]
         string UserName ,
        
         string? UserPassword ,

          string? GoogleId,

         string FirstName ,


         string? LastName ,

        [Phone]
         string? Phone 

   );
}
