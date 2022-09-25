using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductGetPagingRequest : PagingRequest
    {
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}