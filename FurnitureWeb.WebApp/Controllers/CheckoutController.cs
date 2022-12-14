using FurnitureWeb.APICaller.CartItem;
using FurnitureWeb.APICaller.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("~/")]
    public class CheckoutController : Controller
    {
        private readonly ICartItemAPIClient _cartItemAPIClient;
        private readonly IUserAPIClient _userAPIClient;

        public CheckoutController(ICartItemAPIClient cartItemAPIClient, IUserAPIClient userAPIClient)
        {
            _cartItemAPIClient = cartItemAPIClient;
            _userAPIClient = userAPIClient;
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Index()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User)).Data;
            var cartItems = (await _cartItemAPIClient.GetAllCartItemByUser(user.UserId)).Data;
            ViewData["totalItemPrice"] = cartItems.Items.Sum(x => x.Quantity * x.UnitPrice);
            ViewData["shipping"] = (decimal)0;
            ViewData["discount"] = (decimal)0;
            ViewData["totalPrice"] = (decimal)ViewData["totalItemPrice"] + (decimal)ViewData["shipping"];
            return View(cartItems);
        }
    }
}