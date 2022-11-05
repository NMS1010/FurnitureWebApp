using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Discounts
{
    public class DiscountCreateRequest
    {
        [Required]
        [MaxLength(20)]
        public string DiscountCode { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}