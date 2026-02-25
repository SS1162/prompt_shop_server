using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public record AddToCartDTO
{
    [Required]
    public long UserID { get; init; }

    [Required]
    public long ProductsID { get; init; }

    [Required]
    public long PlatformsID { get; init; }

    public long? UserDescription { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public double Price { get; init; }
}
}
