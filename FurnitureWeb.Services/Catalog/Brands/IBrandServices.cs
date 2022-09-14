using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Brands
{
    public interface IBrandServices
    {
        Task<int> Create(BrandCreateRequest request);

        Task<int> Update(BrandUpdateRequest request);

        Task<int> Delete(int brandId);

        Task<PagedResult<BrandViewModel>> RetrieveAll(BrandGetPagingRequest request);

        Task<BrandViewModel> RetrieveById(int brandId);
    }
}