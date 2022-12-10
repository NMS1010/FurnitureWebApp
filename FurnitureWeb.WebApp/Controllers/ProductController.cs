using FurnitureWeb.APICaller.Product;
using FurnitureWeb.Utilities.Constants.Sort;
using FurnitureWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Route("~/")]
    public class ProductController : Controller
    {
        private readonly IProductAPIClient _productAPIClient;

        public ProductController(IProductAPIClient productAPIClient)
        {
            _productAPIClient = productAPIClient;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("products/detail/{productId}")]
        public async Task<IActionResult> GetProductDetail(int productId)
        {
            var product = (await _productAPIClient.GetProductById(productId)).Data;
            return View("ProductDetail", product);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductList(int pageSize = 2, int pageIndex = 1, int sortBy = SORT_BY.BY_NAME_AZ)
        {
            var products = (await _productAPIClient.GetAllProductAsync(new ProductGetPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortBy = sortBy
            })).Data;
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["totalPage"] = Math.Ceiling(products.TotalItem * 1.0 / pageSize);
            return View("ProductList", products);
        }
    }
}