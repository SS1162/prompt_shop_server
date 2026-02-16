using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductDTO
    {
        public long ProductsID { get; set; }
        public long CategoryID { get; set; }
        public string ProductsName { get; set; }
        public string CategoryName { get; set; }
        public string ImgUrl { get; set; }

        public float Price { get; set; }
    }


}
