using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}