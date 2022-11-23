using FurnitureWeb.ViewModels.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public int? DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalItemPrice { get; set; }
        public decimal Shipping { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public int Status { get; set; }
        public string StatusCode { get; set; }
        public string StatusClass { get; set; }
        public int Payment { get; set; }
        public string PaymentMethod { get; set; }
        public int TotalItem { get; set; }
        public PagedResult<OrderItemViewModel> OrderItems { get; set; }
    }
}