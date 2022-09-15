﻿using Domain.Entities;
using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Products
{
    public interface IProductServices : IModifyEntity<ProductCreateRequest, ProductUpdateRequest, int>,
        IRetrieveEntity<ProductViewModel, ProductGetPagingRequest, int>
    {
    }
}