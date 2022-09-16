using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.OrderItems
{
    public class OrderItemUpdateRequest
    {
        [Required]
        public int OrderItemId { get; set; }
    }
}