﻿using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.CartItems
{
    public class CartItemUpdateRequest
    {
        [Required]
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Status { get; set; } = 1;
    }
}