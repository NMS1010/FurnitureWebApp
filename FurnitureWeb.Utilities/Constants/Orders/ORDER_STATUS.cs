using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Orders
{
    public class ORDER_STATUS
    {
        public static int PENDING = 0;
        public static int READY_TO_SHIP = 1;
        public static int ON_THE_WAY = 2;
        public static int DELIVERED = 3;
        public static int CANCELED = 4;
        public static int RETURNED = 5;

        public static Dictionary<int, string> OrderStatus = new Dictionary<int, string>()
        {
            {PENDING, "Đang đợi" },
            {READY_TO_SHIP, "Sẵn sàng chuyển đi" },
            {ON_THE_WAY, "Đang chuyển" },
            {DELIVERED, "Đã hoàn thành" },
            {CANCELED, "Đã huỷ" },
            {RETURNED, "Hoàn trả" },
        };
    }
}