using FurnitureWeb.AdminWebApp.Models;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("~/admin/home")]
    public class HomeController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IOrderAPIClient _orderAPIClient;

        public HomeController(IUserAPIClient userAPIClient, IOrderAPIClient orderAPIClient)
        {
            _userAPIClient = userAPIClient;
            _orderAPIClient = orderAPIClient;
        }

        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Request.Cookies["X-Access-Token-Admin"];
            if (session == null)
            {
                await HttpContext.SignOutAsync("AdminAuth");
                return Redirect("~/admin/login");
            }
            var userRes = await _userAPIClient.GetAllUserAsync(new UserGetPagingRequest());
            var orderRes = await _orderAPIClient.GetAllOrderAsync(new OrderGetPagingRequest());
            var orderStatictis = await _orderAPIClient.GetOverviewStatictis();
            ViewData["orders"] = orderRes.IsSuccesss ? orderRes.Data : new PagedResult<OrderViewModel>();
            ViewData["statistics"] = orderStatictis.IsSuccesss ? orderStatictis.Data : new OrderOverviewViewModel();
            ViewData["totalUsers"] = userRes.IsSuccesss ? userRes.Data.TotalItem : 0;
            ViewData["totalOrders"] = orderRes.IsSuccesss ? orderRes.Data.TotalItem : 0;
            ViewData["totalRevenue"] = orderRes.IsSuccesss ? orderRes.Data.Items.Sum(x => x.TotalPrice) : 0;
            if (!orderRes.IsSuccesss || !orderStatictis.IsSuccesss || !userRes.IsSuccesss)
            {
                ViewData["Error"] = true;
            }
            return View(userRes.IsSuccesss ? userRes.Data : new PagedResult<UserViewModel>());
        }
    }
}