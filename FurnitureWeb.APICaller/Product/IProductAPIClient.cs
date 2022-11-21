using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Product
{
    public interface IProductAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateProduct(ProductCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateProduct(ProductUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<ProductViewModel>>> GetAllProductAsync(ProductGetPagingRequest request);

        Task<CustomAPIResponse<ProductViewModel>> GetProductById(int productId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteProduct(int productId);

        Task<CustomAPIResponse<PagedResult<ProductImageViewModel>>> GetAllProductImageByProductIdAsync(ProductImageGetPagingRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> CreateProductImage(ProductImageCreateSingleRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateProductImage(ProductImageUpdateRequest request);

        Task<CustomAPIResponse<ProductImageViewModel>> GetProductImageById(int productImageId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteProductImage(int productImageId);
    }
}