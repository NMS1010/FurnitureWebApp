using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Orders;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize]
    [Route("~/my-account/")]
    public class UserController : Controller
    {
        private readonly IOrderAPIClient _orderAPIClient;
        private readonly IUserAPIClient _userAPIClient;

        public UserController(IOrderAPIClient orderAPIClient, IUserAPIClient userAPIClient)
        {
            _orderAPIClient = orderAPIClient;
            _userAPIClient = userAPIClient;
        }

        [HttpGet("orders/detail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var order = (await _orderAPIClient.GetOrderById(orderId))?.Data;
            return View("UserOrderDetail", order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrder()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var orders = user?.Orders;
            return View("UserOrder", orders);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            return View(user);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> UpdateInfo([FromForm] UserUpdateRequest request)
        {
            var res = (await _userAPIClient.UpdateUser(request));
            if (!res.IsSuccesss)
            {
                return Redirect("~/my-account?error");
            }
            return Redirect("~/my-account");
        }
    }
}