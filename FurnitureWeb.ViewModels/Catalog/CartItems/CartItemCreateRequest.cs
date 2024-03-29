﻿using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.CartItems
{
    public class CartItemCreateRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public int Status { get; set; } = 1;
    }
}