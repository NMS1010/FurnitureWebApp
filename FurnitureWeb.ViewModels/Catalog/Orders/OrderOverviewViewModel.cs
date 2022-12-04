using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderOverviewViewModel
    {
        public long TotalCanceled { get; set; }
        public long TotalPending { get; set; }
        public long TotalCompleted { get; set; }
        public long TotalReady { get; set; }
        public long TotalReturned { get; set; }
        public long TotalDelivering { get; set; }
    }
}