using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Discounts
{
    public class DiscountGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}