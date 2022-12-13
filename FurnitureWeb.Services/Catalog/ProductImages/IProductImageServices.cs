using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ProductImages
{
    public interface IProductImageServices : IModifyEntity<ProductImageCreateRequest, ProductImageUpdateRequest, int>,
        IRetrieveEntity<ProductImageViewModel, ProductImageGetPagingRequest, int>
    {
        Task<int> CreateSingleImage(ProductImageCreateSingleRequest request);
    }
}