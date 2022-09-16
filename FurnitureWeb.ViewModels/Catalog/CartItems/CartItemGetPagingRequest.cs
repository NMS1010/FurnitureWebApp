using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.CartItems
{
    public class CartItemGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}