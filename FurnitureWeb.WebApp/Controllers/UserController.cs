using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.ViewModels.Catalog.Orders;
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

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrder()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var orders = (await _orderAPIClient.GetAllOrderAsync(new OrderGetPagingRequest()
            {
                UserId = user.UserId
            }))?.Data;
            return View("UserOrder", orders);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            return View(user);
        }
    }
}