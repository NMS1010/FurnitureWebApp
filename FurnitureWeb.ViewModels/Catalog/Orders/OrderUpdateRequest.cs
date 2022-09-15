using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderUpdateRequest
    {
        [Required]
        public int OrderId { get; set; }

        public int Status { get; set; }
    }
}