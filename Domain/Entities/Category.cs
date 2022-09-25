using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Content { get; set; }

        public string ImagePath { get; set; }

        public HashSet<Product> Products { get; set; }
    }
}