﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }

        public HashSet<Order> Orders { get; set; }
    }
}