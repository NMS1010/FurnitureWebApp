using FurnitureWeb.APICaller.CartItem;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize]
    [Route("~/cart/")]
    public class CartController : Controller
    {
        private readonly ICartItemAPIClient _cartItemAPIClient;
        private readonly IUserAPIClient _userAPIClient;

        public CartController(ICartItemAPIClient cartItemAPIClient, IUserAPIClient userAPIClient)
        {
            _cartItemAPIClient = cartItemAPIClient;
            _userAPIClient = userAPIClient;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetCartItem()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var cartItems = (await _cartItemAPIClient.GetAllCartItemByUser(user.UserId))?.Data;
            ViewData["total"] = cartItems?.Items.Sum(x => x.Quantity * x.UnitPrice);
            return View("Index", cartItems);
        }

        [HttpPost("update-quantity")]
        public async Task<IActionResult> UpdateQuantity(CartItemUpdateRequest request)
        {
            var res = await _cartItemAPIClient.UpdateItemQuantity(request);
            return Ok(res);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            var res = await _cartItemAPIClient.DeleteCartItem(cartItemId);
            return RedirectToAction(nameof(GetCartItem));
        }

        [HttpGet("add")]
        public async Task<IActionResult> AddProductToCart(int productId, int quantity = 1)
        {
            var res = await _cartItemAPIClient.AddProductToCart(new CartItemCreateRequest()
            {
                ProductId = productId,
                UserId = (await _userAPIClient.RetrieveByClaimsPrincipal(User)).Data.UserId,
                Quantity = quantity,
                Status = 1
            });
            return Ok(res.Data);
        }
    }
}