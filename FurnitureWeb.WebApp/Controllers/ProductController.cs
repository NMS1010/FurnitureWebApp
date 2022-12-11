using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.APICaller.Category;
using FurnitureWeb.APICaller.Product;
using FurnitureWeb.Utilities.Constants.Paging;
using FurnitureWeb.Utilities.Constants.Sort;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Route("~/products/")]
    public class ProductController : Controller
    {
        private readonly IProductAPIClient _productAPIClient;
        private readonly ICategoryAPIClient _categoryAPIClient;
        private readonly IBrandAPIClient _brandAPIClient;

        public ProductController(IProductAPIClient productAPIClient, IBrandAPIClient brandAPIClient, ICategoryAPIClient categoryAPIClient)
        {
            _productAPIClient = productAPIClient;
            _brandAPIClient = brandAPIClient;
            _categoryAPIClient = categoryAPIClient;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("detail/{productId}")]
        public async Task<IActionResult> GetProductDetail(int productId)
        {
            var product = (await _productAPIClient.GetProductById(productId)).Data;
            return View("ProductDetail", product);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProductList(ProductGetPagingRequest request)
        {
            if (request.PageSize == PAGE_SIZE.MaxPageSize)
                request.PageSize = PAGE_SIZE.DefaultPageSize;
            var products = (await _productAPIClient.GetAllProductAsync(request)).Data;
            var categories = (await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest())).Data;
            var brands = (await _brandAPIClient.GetAllBrandAsync(new BrandGetPagingRequest())).Data;

            ViewData["pageIndex"] = request.PageIndex;
            ViewData["pageSize"] = request.PageSize;
            ViewData["sortBy"] = request.SortBy;
            ViewData["keyword"] = request.Keyword;
            ViewData["categoryId"] = request.CategoryId;
            ViewData["brandId"] = request.BrandId;
            ViewData["minPrice"] = request.MinPrice;
            ViewData["maxPrice"] = request.MaxPrice;
            ViewData["totalPage"] = Convert.ToInt32(Math.Ceiling(products.TotalItem * 1.0 / request.PageSize));
            ViewData["categories"] = categories;
            ViewData["brands"] = brands;
            return View("ProductList", products);
        }
    }
}