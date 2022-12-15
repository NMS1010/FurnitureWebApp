using FurnitureWeb.APICaller.CartItem;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Orders;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Catalog.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize]
    [Route("~/")]
    public class CheckoutController : Controller
    {
        private readonly ICartItemAPIClient _cartItemAPIClient;
        private readonly IUserAPIClient _userAPIClient;
        private readonly IOrderAPIClient _orderAPIClient;

        public CheckoutController(ICartItemAPIClient cartItemAPIClient, IUserAPIClient userAPIClient, IOrderAPIClient orderAPIClient)
        {
            _cartItemAPIClient = cartItemAPIClient;
            _userAPIClient = userAPIClient;
            _orderAPIClient = orderAPIClient;
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Index()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User)).Data;
            var cartItems = (await _cartItemAPIClient.GetAllCartItemByUser(user.UserId))?.Data;
            ViewData["totalItemPrice"] = cartItems?.Items.Sum(x => x.Quantity * x.UnitPrice);
            ViewData["shipping"] = (decimal)0;
            ViewData["discount"] = (decimal)0;
            ViewData["totalPrice"] = (decimal)ViewData["totalItemPrice"] + (decimal)ViewData["shipping"];
            return View(cartItems);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromForm] OrderCreateRequest request)
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            request.Status = ORDER_STATUS.PENDING;
            request.UserId = user.UserId;

            var res = await _orderAPIClient.CreateOrder(request);
            if (!res.IsSuccesss)
            {
                return Redirect("~/cart/items?error");
            }
            return Redirect("~/cart/items?success");
        }
    }
}