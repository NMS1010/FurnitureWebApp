using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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

        public async Task<IActionResult> Index(CustomAPIResponse<NoContentAPIResponse> response = null)
        {
            if (response != null && !response.IsSuccesss)
                ViewData["Error"] = response.Errors;
            var res = await _brandAPIClient.GetAllAsync(new BrandGetPagingRequest());
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            var res = await _brandAPIClient.CreateBrand(request);
            return RedirectToAction(nameof(Index), res);
        }

        [HttpGet("delete/{brandId}")]
        public async Task<IActionResult> Delete(int brandId)
        {
            var res = await _brandAPIClient.Delete(brandId);
            return RedirectToAction(nameof(Index), res);
        }

        [Route("get/{brandId}")]
        public async Task<IActionResult> Get(int brandId)
        {
            var res = await _brandAPIClient.GetById(brandId);
            if (!res.IsSuccesss)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(BrandUpdateRequest request)
        {
            var res = await _brandAPIClient.UpdateBrand(request);
            return RedirectToAction(nameof(Index), res);
        }
    }
}