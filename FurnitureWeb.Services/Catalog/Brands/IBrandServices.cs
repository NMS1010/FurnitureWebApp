using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Brands;

namespace FurnitureWeb.Services.Catalog.Brands
{
    public interface IBrandServices : IModifyEntity<BrandCreateRequest, BrandUpdateRequest, int>,
        IRetrieveEntity<BrandViewModel, BrandGetPagingRequest, int>
    {
    }
}