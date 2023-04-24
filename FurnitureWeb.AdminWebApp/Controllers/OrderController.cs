using FurnitureWeb.APICaller.Order;
using FurnitureWeb.Utilities.Constants.Orders;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/orders/")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderAPIClient _orderAPIClient;

        public OrderController(IOrderAPIClient orderAPIClient)
        {
            _orderAPIClient = orderAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _orderAPIClient.GetAllOrderAsync(new OrderGetPagingRequest());
            if (error || !res.IsSuccess)
                ViewData["Error"] = res.Errors;
            return View(res.Data);
        }

        [Route("get/{type?}")]
        public async Task<IActionResult> GetAll(string type = null)
        {
            var res = await _orderAPIClient.GetAllOrderAsync(new OrderGetPagingRequest());
            PagedResult<OrderViewModel> result = new PagedResult<OrderViewModel>()
            {
                Items = new System.Collections.Generic.List<OrderViewModel>(),
                TotalItem = 0
            };
            if (type == "new")
            {
                result.Items.AddRange(res.Data.Items.FindAll(o => o.Status == ORDER_STATUS.PENDING));
                result.TotalItem = result.Items.Count;
            }
            else if (type == "delivered")
            {
                result.Items.AddRange(res.Data.Items.FindAll(o => o.Status == ORDER_STATUS.DELIVERED));
                result.TotalItem = result.Items.Count;
            }
            else
            {
                result.Items.AddRange(res.Data.Items);
                result.TotalItem = result.Items.Count;
            }
            return View("Index", result);
        }

        [Route("detail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var res = await _orderAPIClient.GetOrderById(orderId);
            if (!res.IsSuccess)
            {
                ViewData["Error"] = res.Errors;
            }
            return View("OrderDetail", res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(OrderUpdateRequest request)
        {
            var res = await _orderAPIClient.UpdateOrder(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }
    }
}