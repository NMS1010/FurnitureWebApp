using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.OrderItems
{
    public class OrderItemGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}