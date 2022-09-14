using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }

        public string Origin { get; set; }

        public DateTime DateCreated { get; set; }

        public string ImagePath { get; set; }

        public string CategoryName { get; set; }

        public string BrandName { get; set; }
    }
}