using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.ProductImages
{
    public class ProductImageGetPagingRequest : PagingRequest
    {
        public int ProductId { get; set; }
    }
}