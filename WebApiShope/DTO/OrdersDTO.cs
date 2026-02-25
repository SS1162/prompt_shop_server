using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{public record OrdersDTO
{
    [Required]
    public long UserID { get; init; }

    [Required]
    public float OrderSum { get; init; }

    [Required]
    public long BasicID { get; init; }

    [Required]
    public List<AddToCartDTO> Products { get; init; } = new();
}}