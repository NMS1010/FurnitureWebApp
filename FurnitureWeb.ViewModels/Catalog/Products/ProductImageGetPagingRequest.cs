﻿using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductImageGetPagingRequest : PagingRequest
    {
        public int ProductId { get; set; }
    }
}