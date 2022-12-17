using FurnitureWeb.APICaller.User;
using FurnitureWeb.APICaller.WishItem;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize]
    [Route("~/")]
    public class WishController : Controller
    {
        private readonly IWishItemAPIClient _wishItemAPIClient;
        private readonly IUserAPIClient _userAPIClient;

        public WishController(IWishItemAPIClient wishItemAPIClient, IUserAPIClient userAPIClient)
        {
            _wishItemAPIClient = wishItemAPIClient;
            _userAPIClient = userAPIClient;
        }

        [HttpGet("wish-list")]
        public async Task<IActionResult> GetAllWishItem()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var wishItems = (await _wishItemAPIClient.GetAllWishItemByUser(user?.UserId))?.Data;
            return View("Index", wishItems ?? new PagedResult<WishItemViewModel>());
        }

        [HttpGet("add-wish")]
        public async Task<IActionResult> AddProductToWish(int productId)
        {
            var res = await _wishItemAPIClient.AddProductToWish(new WishItemCreateRequest()
            {
                ProductId = productId,
                UserId = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data?.UserId,
                Status = 1
            });
            return Ok(res.Data);
        }

        [HttpGet("delete-wish")]
        public async Task<IActionResult> DeleteWishItem(int wishItemId)
        {
            var res = await _wishItemAPIClient.DeleteWishItem(wishItemId);
            return RedirectToAction(nameof(GetAllWishItem));
        }
    }
}