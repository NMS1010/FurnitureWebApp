using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ReviewItems
{
    public interface IReviewItemServices : IModifyEntity<ReviewItemCreateRequest, ReviewItemUpdateRequest, int>,
        IRetrieveEntity<ReviewItemViewModel, ReviewItemGetPagingRequest, int>
    {
        Task<int> ChangeReviewStatus(int reviewItemId);

        Task<PagedResult<ReviewItemViewModel>> RetrieveReviewsByUser(string userId, int productId);
    }
}