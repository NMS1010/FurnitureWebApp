using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
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

        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            await _brandAPIClient.CreateBrand(request);
            return RedirectToAction(nameof(Index));
        }
    }
}