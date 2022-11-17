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
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateBrand(BrandCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateBrand(BrandUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<BrandViewModel>>> GetAllAsync(BrandGetPagingRequest request);

        Task<CustomAPIResponse<BrandViewModel>> GetById(int brandId);

        Task<CustomAPIResponse<NoContentAPIResponse>> Delete(int brandId);
    }
}