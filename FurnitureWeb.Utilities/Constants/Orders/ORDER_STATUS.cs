using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Orders
{
    public enum ORDER_STATUS
    {
        PENDING,
        READY_TO_SHIP,
        ON_THE_WAY,
        DELIVERED,
        CANCELED,
        RETURNED
    }
}