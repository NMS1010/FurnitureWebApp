using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Brand
{
    public interface IBrandAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateBrand(BrandCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateBrand(BrandUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<BrandViewModel>>> GetAllBrandAsync(BrandGetPagingRequest request);

        Task<CustomAPIResponse<BrandViewModel>> GetBrandById(int brandId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteBrand(int brandId);
    }
}