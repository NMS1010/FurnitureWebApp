using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/brand/")]
    public class BrandController : Controller
    {
        private readonly IBrandAPIClient _brandAPIClient;

        public BrandController(IBrandAPIClient brandAPIClient)
        {
            _brandAPIClient = brandAPIClient;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _brandAPIClient.GetAllAsync(new BrandGetPagingRequest()));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            await _brandAPIClient.CreateBrand(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{brandId}")]
        public async Task<IActionResult> Delete(int brandId)
        {
            await _brandAPIClient.Delete(brandId);
            return RedirectToAction(nameof(Index));
        }

        [Route("get/{brandId}")]
        public async Task<IActionResult> Get(int brandId)
        {
            return Ok(await _brandAPIClient.GetById(brandId));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(BrandUpdateRequest request)
        {
            await _brandAPIClient.UpdateBrand(request);
            return RedirectToAction(nameof(Index));
        }
    }
}