using System;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Discounts
{
    public class DiscountViewModel
    {
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
    }
}