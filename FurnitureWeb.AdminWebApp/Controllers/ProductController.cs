using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.APICaller.Category;
using FurnitureWeb.APICaller.Product;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("~/admin/products")]
    public class ProductController : Controller
    {
        private readonly IProductAPIClient _productAPIClient;
        private readonly IBrandAPIClient _brandAPIClient;
        private readonly ICategoryAPIClient _categoryAPIClient;

        public ProductController(IProductAPIClient productAPIClient, IBrandAPIClient brandAPIClient, ICategoryAPIClient categoryAPIClient)
        {
            _productAPIClient = productAPIClient;
            _brandAPIClient = brandAPIClient;
            _categoryAPIClient = categoryAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _productAPIClient.GetAllProductAsync(new ProductGetPagingRequest());
            if (error || !res.IsSuccess)
                ViewData["Error"] = res.Errors;
            else
                ViewData["Error"] = null;
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            var res = await _productAPIClient.CreateProduct(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [HttpGet("add/get")]
        public async Task<IActionResult> Add()
        {
            ViewData["Categories"] = (await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest())).Data.Items;
            ViewData["Brands"] = (await _brandAPIClient.GetAllBrandAsync(new BrandGetPagingRequest())).Data.Items;
            return View("AddProduct");
        }

        [HttpGet("edit/get/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var res = await _productAPIClient.GetProductById(productId);
            ViewData["Categories"] = (await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest())).Data.Items;
            ViewData["Brands"] = (await _brandAPIClient.GetAllBrandAsync(new BrandGetPagingRequest())).Data.Items;
            return View("AddProduct", res.Data);
        }

        [HttpGet("images/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var res = await _productAPIClient.GetAllProductImageByProductIdAsync(new ProductImageGetPagingRequest() { ProductId = productId });
            return View("ProductImages", res.Data);
        }

        [HttpGet("images/get/{productImageId}")]
        public async Task<IActionResult> GetImageById(int productImageId)
        {
            var res = await _productAPIClient.GetProductImageById(productImageId);
            if (!res.IsSuccess)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpGet("delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var res = await _productAPIClient.DeleteProduct(productId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [HttpGet("get/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var res = await _productAPIClient.GetProductById(productId);
            if (!res.IsSuccess)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("images/edit")]
        public async Task<IActionResult> EditImage(ProductImageUpdateRequest request)
        {
            var res = await _productAPIClient.UpdateProductImage(request);
            return Redirect($"/admin/products/images/{request.ProductId}");
        }

        [HttpGet("images/{productId}/delete/{productImageId}")]
        public async Task<IActionResult> DeleteImage(int productId, int productImageId)
        {
            var res = await _productAPIClient.DeleteProductImage(productImageId);
            return Redirect($"/admin/products/images/{productId}");
        }

        [HttpPost("images/add")]
        public async Task<IActionResult> AddImage(ProductImageCreateSingleRequest request)
        {
            var res = await _productAPIClient.CreateProductImage(request);
            return Redirect($"/admin/products/images/{request.ProductId}");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            var res = await _productAPIClient.UpdateProduct(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }
    }
}