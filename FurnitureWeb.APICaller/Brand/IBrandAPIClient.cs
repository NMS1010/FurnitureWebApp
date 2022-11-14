using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Brand
{
    public interface IBrandAPIClient
    {
        Task<bool> CreateBrand(BrandCreateRequest request);

        Task<bool> UpdateBrand(BrandUpdateRequest request);

        Task<PagedResult<BrandViewModel>> GetAllAsync(BrandGetPagingRequest request);

        Task<BrandViewModel> GetById(int brandId);

        Task<bool> Delete(int brandId);
    }
}