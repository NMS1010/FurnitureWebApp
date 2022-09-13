using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Origin { get; set; }
        public string ImagePath { get; set; }
        public long ImageSize { get; set; }

        public HashSet<Product> Products { get; set; }
    }
}