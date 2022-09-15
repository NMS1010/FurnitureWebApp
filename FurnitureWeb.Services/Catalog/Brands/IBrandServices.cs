using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Brands
{
    public interface IBrandServices : IModifyEntity<BrandCreateRequest, BrandUpdateRequest, int>,
        IRetrieveEntity<BrandViewModel, BrandGetPagingRequest, int>
    {
    }
}