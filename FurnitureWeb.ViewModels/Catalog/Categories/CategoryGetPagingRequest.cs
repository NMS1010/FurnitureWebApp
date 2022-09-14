using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Categories
{
    public class CategoryGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}