using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Discounts
{
    public interface IDiscountServices : IModifyEntity<DiscountCreateRequest, DiscountUpdateRequest, int>,
        IRetrieveEntity<DiscountViewModel, DiscountGetPagingRequest, int>
    {
    }
}