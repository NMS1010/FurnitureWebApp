using Domain.Entities;
using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ProductImages
{
    public interface IProductImageServices : IModifyEntity<ProductImageCreateRequest, ProductImageUpdateRequest, int>,
        IRetrieveEntity<ProductImageViewModel, ProductImageGetPagingRequest, int>
    {
        Task<int> CreateSingleImage(ProductImageCreateSingleRequest request);
    }
}