using Domain.Entities;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.ReviewItem;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Orders;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Authorize]
    [Route("~/reviews/")]
    public class ReviewController : Controller
    {
        private readonly IOrderAPIClient _orderAPIClient;

        private readonly IReviewItemAPIClient _reviewAPIClient;
        private readonly IUserAPIClient _userAPIClient;

        public ReviewController(IOrderAPIClient orderAPIClient, IReviewItemAPIClient reviewAPIClient, IUserAPIClient userAPIClient)
        {
            _orderAPIClient = orderAPIClient;
            _reviewAPIClient = reviewAPIClient;
            _userAPIClient = userAPIClient;
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> Index(int orderId, int productId, int reviewItemId = 0)
        {
            var order = (await _orderAPIClient.GetOrderById(orderId))?.Data;
            if (order.Status != ORDER_STATUS.DELIVERED)
                return Redirect($"orders/detail/${orderId}?error");
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var reviewItems = (await _reviewAPIClient.GetReviewItemByUser(user.UserId, productId))?.Data;
            ViewData["orderId"] = orderId;
            ViewData["productId"] = productId;
            if (reviewItemId != 0)
            {
                var res = await _reviewAPIClient.GetReviewItemById(reviewItemId);
                if (res == null || !res.IsSuccesss)
                {
                    ViewData["Error"] = "Error";
                }
                else
                {
                    ViewData["productReview"] = res.Data;
                }
            }
            return View(reviewItems);
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateReview([FromForm] ReviewItemCreateRequest request)
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            if (user == null)
                return Redirect("~/signout");
            request.UserId = user?.UserId;
            var res = await _reviewAPIClient.CreateReviewItem(request);
            string status = "";
            if (res == null || !res.IsSuccesss)
            {
                status = "?error";
            }
            return Redirect($"~/reviews/{request.OrderId}/{request.ProductId}" + status);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> UpdateReview([FromForm] ReviewItemUpdateRequest request)
        {
            var res = await _reviewAPIClient.UpdateReviewItem(request);
            string status = "";
            if (res == null || !res.IsSuccesss)
            {
                status = "?error";
            }
            return Redirect($"~/reviews/{request.OrderId}/{request.ProductId}" + status);
        }
    }
}