﻿using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using System;

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
        public string StatusCode { get; set; }
        public string StatusClass { get; set; }

        public string Origin { get; set; }

        public DateTime DateCreated { get; set; }

        public string ImagePath { get; set; }

        public string CategoryName { get; set; }

        public string BrandName { get; set; }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int TotalPurchased { get; set; }
        public int AverageRating { get; set; }
        public PagedResult<ProductImageViewModel> SubImages { get; set; }
        public PagedResult<ReviewItemViewModel> ProductReview { get; set; }
    }
}