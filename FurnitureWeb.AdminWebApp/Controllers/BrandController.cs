using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/brands/")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandAPIClient _brandAPIClient;

        public BrandController(IBrandAPIClient brandAPIClient)
        {
            _brandAPIClient = brandAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _brandAPIClient.GetAllBrandAsync(new BrandGetPagingRequest());
            if (error || !res.IsSuccess)
                ViewData["Error"] = res.Errors;
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            var res = await _brandAPIClient.CreateBrand(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [HttpGet("delete/{brandId}")]
        public async Task<IActionResult> Delete(int brandId)
        {
            var res = await _brandAPIClient.DeleteBrand(brandId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [Route("get/{brandId}")]
        public async Task<IActionResult> GetById(int brandId)
        {
            var res = await _brandAPIClient.GetBrandById(brandId);
            if (!res.IsSuccess)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(BrandUpdateRequest request)
        {
            var res = await _brandAPIClient.UpdateBrand(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }
    }
}