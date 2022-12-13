using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Products;

namespace FurnitureWeb.Services.Catalog.Products
{
    public interface IProductServices : IModifyEntity<ProductCreateRequest, ProductUpdateRequest, int>,
        IRetrieveEntity<ProductViewModel, ProductGetPagingRequest, int>
    {
    }
}