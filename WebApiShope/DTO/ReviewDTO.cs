using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReviewDTO
    {
        public ReviewDTO() { }
        [Required]
        public long ReviewId { get; set; }

        //[Required]
        public long OrderId { get; set; }

        [Required]
        public int Stars { get; set; }

        public string ReviewText { get; set; }

        public string ReviewImg { get; set; }
    }
        
   
}
