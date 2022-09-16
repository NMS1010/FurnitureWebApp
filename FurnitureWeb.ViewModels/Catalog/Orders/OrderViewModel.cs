﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalItemPrice { get; set; }
        public decimal Shipping { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public int Status { get; set; }
    }
}