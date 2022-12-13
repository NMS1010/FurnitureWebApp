using FurnitureWeb.ViewModels.Catalog.Wishtems;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.WishItem
{
    public interface IWishItemAPIClient
    {
        Task<CustomAPIResponse<string>> AddProductToWish(WishItemCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteWishItem(int wishItemId);

        Task<CustomAPIResponse<PagedResult<WishItemViewModel>>> GetAllWishItemByUser(string userId);
    }
}