using FurnitureWeb.APICaller.CartItem;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Services.External.Paypal;
using FurnitureWeb.Utilities.Constants.Orders;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Catalog.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IPaypalService _paypalService;

        public CheckoutController(ICartItemAPIClient cartItemAPIClient, IUserAPIClient userAPIClient, IOrderAPIClient orderAPIClient, IPaypalService paypalService)
        {
            _cartItemAPIClient = cartItemAPIClient;
            _userAPIClient = userAPIClient;
            _orderAPIClient = orderAPIClient;
            _paypalService = paypalService;
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

        [HttpGet("checkout/paypal/success")]
        public async Task<IActionResult> PaypalResponse(string payerId, string paymentId)
        {
            try
            {
                var orderCreateReq = JsonConvert.DeserializeObject<OrderCreateRequest>(HttpContext.Request.Cookies["X-Token-OrderCreateReq"]);
                var exePayment = _paypalService.ExecutePayment(payerId, paymentId);
                if (!exePayment)
                    return Redirect("~/cart/items?error");
                var res = await _orderAPIClient.CreateOrder(orderCreateReq);
                if (!res.IsSuccesss)
                {
                    return Redirect("~/cart/items?error");
                }
                return Redirect("~/cart/items?success");
            }
            catch
            {
                return Redirect("~/cart/items?error");
            }
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromForm] OrderCreateRequest request)
        {
            var user = (await _userAPIClient.RetrieveByClaimsPrincipal(User))?.Data;
            var cartItems = (await _cartItemAPIClient.GetAllCartItemByUser(user.UserId))?.Data;
            request.Status = ORDER_STATUS.PENDING;
            request.UserId = user?.UserId;
            if (request.Payment == ORDER_PAYMENT.PAYPAL)
            {
                string approvedUrl = _paypalService.PaypalCheckout(cartItems, request, $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}");
                if (approvedUrl != null)
                {
                    HttpContext.Response.Cookies.Append("X-Token-OrderCreateReq", JsonConvert.SerializeObject(request), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Lax });

                    return Redirect(approvedUrl);
                }
                return Redirect("~/cart/items?error");
            }
            var res = await _orderAPIClient.CreateOrder(request);
            if (!res.IsSuccesss)
            {
                return Redirect("~/cart/items?error");
            }
            return Redirect("~/cart/items?success");
        }
    }
}