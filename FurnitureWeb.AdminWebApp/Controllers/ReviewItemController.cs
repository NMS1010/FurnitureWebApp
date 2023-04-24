using FurnitureWeb.APICaller.ReviewItem;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/review-items/")]
    [Authorize(Roles = "Admin")]
    public class ReviewItemController : Controller
    {
        private readonly IReviewItemAPIClient _reviewItemAPIClient;

        public ReviewItemController(IReviewItemAPIClient reviewItemAPIClient)
        {
            _reviewItemAPIClient = reviewItemAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _reviewItemAPIClient.GetAllReviewItemAsync(new ReviewItemGetPagingRequest());
            if (error || !res.IsSuccess)
                ViewData["Error"] = res.Errors;
            return View(res.Data);
        }

        [HttpGet("delete/{reviewItemId}")]
        public async Task<IActionResult> Delete(int reviewItemId)
        {
            var res = await _reviewItemAPIClient.DeleteReviewItem(reviewItemId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }
    }
}