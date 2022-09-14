using Domain.Entities;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Products
{
    public interface IProductServices
    {
        Task<int> Create(ProductCreateRequest request);

        Task<PagedResult<ProductViewModel>> RetrieveAll(ProductGetPagingRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<ProductViewModel> RetrieveByProductId(int productId);

        Task<int> AddImages(ProductImageCreateRequest request);

        Task<PagedResult<ProductImage>> RetrieveImages(ProductImageGetPagingRequest request);

        Task<ProductImage> RetrieveImageById(int productImageId);

        Task<int> DeleteImage(int productImageId);

        Task<int> UpdateImage(ProductImageUpdateRequest request);
    }
}