using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Orders
{
    public class ORDER_PAYMENT
    {
        public static int PAID = 0;
        public static int COD = 1;

        public static Dictionary<int, string> OrderPayment = new Dictionary<int, string>()
        {
            {PAID, "Đã thanh toán" },
            {COD, "Thanh toán khi nhận hàng" }
        };
    }
}