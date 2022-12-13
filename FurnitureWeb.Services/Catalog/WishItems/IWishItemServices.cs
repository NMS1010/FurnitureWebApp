using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.WishItems
{
    public interface IWishItemServices : IModifyEntity<WishItemCreateRequest, WishItemUpdateRequest, int>,
        IRetrieveEntity<WishItemViewModel, WishItemGetPagingRequest, int>
    {
        Task<PagedResult<WishItemViewModel>> RetrieveWishByUserId(string userId);

        Task<string> AddProductToWish(WishItemCreateRequest request);
    }
}