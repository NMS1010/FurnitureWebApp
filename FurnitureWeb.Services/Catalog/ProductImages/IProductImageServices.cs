using Domain.Entities;
using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Services.Catalog.ProductImages
{
    public interface IProductImageServices : IModifyEntity<ProductImageCreateRequest, ProductImageUpdateRequest, int>,
        IRetrieveEntity<ProductImage, ProductImageGetPagingRequest, int>
    {
    }
}