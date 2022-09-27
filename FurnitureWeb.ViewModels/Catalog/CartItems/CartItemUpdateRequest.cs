using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.CartItems
{
    public class CartItemUpdateRequest
    {
        [Required]
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}