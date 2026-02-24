using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record OrderDetielsDTO
    {
        [Required]
        public long OrderID { get; init; }

        [Required]
        public long UserID { get; init; }

        public long ReviewId { get; init; }

        public string ReviewImg { get; init; } = string.Empty;

        public double Stars { get; init; }

        public string ReviewNote { get; init; } = string.Empty;

        [Required]
        public string SiteName { get; init; } = string.Empty;

        public string SiteTypeName { get; init; } = string.Empty;

        public string SiteTypeDescreption { get; init; } = string.Empty;

        public string Prompt { get; init; } = string.Empty;

        [Required]
        public List<AddToCartDTO> Products { get; init; } = new();
    }
}
