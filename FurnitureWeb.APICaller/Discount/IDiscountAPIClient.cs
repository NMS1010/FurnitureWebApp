using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Discount
{
    public interface IDiscountAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateDiscount(DiscountCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateDiscount(DiscountUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<DiscountViewModel>>> GetAllDiscountAsync(DiscountGetPagingRequest request);

        Task<CustomAPIResponse<DiscountViewModel>> GetDiscountById(int discountId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteDiscount(int discountId);

        Task<CustomAPIResponse<string>> ApplyDiscount(string discountCode);
    }
}