using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.ReviewItem
{
    public interface IReviewItemAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateReviewItem(ReviewItemCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateReviewItem(ReviewItemUpdateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> ChangeStatusReviewItem(int reviewItemId);

        Task<CustomAPIResponse<PagedResult<ReviewItemViewModel>>> GetAllReviewItemAsync(ReviewItemGetPagingRequest request);

        Task<CustomAPIResponse<ReviewItemViewModel>> GetReviewItemById(int reviewItemId);

        Task<CustomAPIResponse<PagedResult<ReviewItemViewModel>>> GetReviewItemByUser(string userId, int productId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteReviewItem(int reviewItemId);
    }
}