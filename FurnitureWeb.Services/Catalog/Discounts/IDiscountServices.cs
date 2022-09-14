using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Discounts
{
    public interface IDiscountServices
    {
        Task<int> Create(DiscountCreateRequest request);

        Task<PagedResult<DiscountViewModel>> RetrieveAll(DiscountGetPagingRequest request);

        Task<int> Update(DiscountUpdateRequest request);

        Task<int> Delete(int discountId);

        Task<DiscountViewModel> RetrieveById(int discountId);
    }
}