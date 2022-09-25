using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderCreateRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int DiscountId { get; set; }

        [Required]
        public decimal TotalItemPrice { get; set; }

        [Required]
        public decimal Shipping { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int Status { get; set; }
    }
}