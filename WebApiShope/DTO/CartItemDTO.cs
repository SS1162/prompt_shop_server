using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CartItemDTO
    {
        public long CartID { get; init; }

        public string ProductsName { get; init; } = string.Empty;

        public float Price { get; init; }

        public string CategoryName { get; init; } = string.Empty;

        public string ImgUrl { get; init; } = string.Empty;

        public string CategoryDescreption { get; init; } = string.Empty;

        public string? UserDescreption { get; init; } = string.Empty;

        public int Valid { get; init; }


        public string PlatformName { get; init; } = string.Empty;

        public long ProductID { get; init; }

        public long? UserDescreptionID { get; init; }

        public long PlatformID { get; init; }
    }
}
