using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record FullOrderDTO
    {
        [Required]
        public long OrderID { get; init; }

        [Required]
        public float OrderSum { get; init; }

        [Required]
        public DateOnly OrderDate { get; init; }

        [Required]
        public string StatusName { get; init; } = string.Empty;

        public string SiteName { get; init; } = string.Empty;

        public int ProductCount { get; init; }

        public long? ReviewId { get; init; }
    }
}
