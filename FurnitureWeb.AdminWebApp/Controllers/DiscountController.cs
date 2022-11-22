using FurnitureWeb.APICaller.Discount;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/discounts/")]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountAPIClient _discountAPIClient;

        public DiscountController(IDiscountAPIClient discountAPIClient)
        {
            _discountAPIClient = discountAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _discountAPIClient.GetAllDiscountAsync(new DiscountGetPagingRequest());
            if (error || !res.IsSuccesss)
                ViewData["Error"] = res.Errors;
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(DiscountCreateRequest request)
        {
            var res = await _discountAPIClient.CreateDiscount(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccesss });
        }

        [HttpGet("delete/{discountId}")]
        public async Task<IActionResult> Delete(int discountId)
        {
            var res = await _discountAPIClient.DeleteDiscount(discountId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccesss });
        }

        [Route("get/{discountId}")]
        public async Task<IActionResult> GetById(int discountId)
        {
            var res = await _discountAPIClient.GetDiscountById(discountId);
            if (!res.IsSuccesss)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(DiscountUpdateRequest request)
        {
            var res = await _discountAPIClient.UpdateDiscount(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccesss });
        }
    }
}