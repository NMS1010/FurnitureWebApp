﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public string Origin { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Status { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public HashSet<Review> Reviews { get; set; }
        public HashSet<ProductImage> ProductImages { get; set; }
        public HashSet<CartItem> CartItems { get; set; }
        public HashSet<OrderItem> OrderItems { get; set; }
        public HashSet<WishListItem> WishLists { get; set; }
    }
}