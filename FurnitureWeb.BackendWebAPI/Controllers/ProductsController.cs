using Domain.Entities;
using FurnitureWeb.Services.Catalog.Products;
using FurnitureWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductsController(IProductServices productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAllPaging([FromQuery] ProductGetPagingRequest request)
        {
            var products = await _productService.RetrieveAll(request);
            if (products.TotalItem == 0)
                return BadRequest();
            return Ok(products);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int productId = await _productService.Create(request);
            if (productId <= 0)
                return BadRequest();
            var product = await _productService.RetrieveByProductId(productId);
            return CreatedAtAction(nameof(RetrieveById), new { Id = productId }, product);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> RetrieveById(int productId)
        {
            var product = await _productService.RetrieveByProductId(productId);
            if (product == null)
                return NotFound($"Can't find product with id: {productId}");
            return Ok(product);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productService.Update(request);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> Delete(int producId)
        {
            int records = await _productService.Delete(producId);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }

        ////Product Images
        [HttpGet("product/images/all")]
        public async Task<IActionResult> RetrieveImageAllPaging([FromQuery] ProductImageGetPagingRequest request)
        {
            var productImages = await _productService.RetrieveImages(request);
            if (productImages == null)
                return BadRequest();
            return Ok(productImages);
        }

        [HttpGet("product/images/{productImageId}")]
        public async Task<IActionResult> RetrieveImageById(int productImageId)
        {
            var productImage = await _productService.RetrieveImageById(productImageId);
            if (productImage == null)
                return NotFound($"Can't find product with id: {productImageId}");
            return Ok(productImage);
        }

        [HttpPost("product/images/add")]
        public async Task<IActionResult> CreateImages([FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int count = await _productService.AddImages(request);
            if (count <= 0)
                return BadRequest();
            var pagingRequest = new ProductImageGetPagingRequest() { ProductId = request.ProductId };
            var productImages = await _productService.RetrieveImages(pagingRequest);
            return CreatedAtAction(nameof(RetrieveImageAllPaging), new { request = pagingRequest }, productImages);
        }

        [HttpPut("product/images/update")]
        public async Task<IActionResult> UpdateImage([FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productService.UpdateImage(request);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("product/images/delete/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productService.DeleteImage(imageId);
            if (records == 0)
                return BadRequest();
            return Ok();
        }
    }
}