using FurnitureWeb.APICaller.Category;
using FurnitureWeb.APICaller.Product;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductAPIClient _productAPIClient;
        private readonly ICategoryAPIClient _categoryAPIClient;

        public HomeController(IProductAPIClient productAPIClient, ICategoryAPIClient categoryAPIClient)
        {
            _productAPIClient = productAPIClient;
            _categoryAPIClient = categoryAPIClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productAPIClient.GetAllProductAsync(new ProductGetPagingRequest() { PageSize = 10 });
            var categories = await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest());
            ViewData["categories"] = categories.Data;
            return View(products.Data);
        }
    }
}