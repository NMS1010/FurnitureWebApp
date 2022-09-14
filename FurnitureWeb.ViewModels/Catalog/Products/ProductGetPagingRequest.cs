using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}