﻿using FurnitureWeb.Services.Catalog.ProductImages;
using FurnitureWeb.Services.Catalog.Products;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IProductImageServices _productImageService;

        public ProductsController(IProductServices productService, IProductImageServices productImageService)
        {
            _productService = productService;
            _productImageService = productImageService;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> RetrieveAllPaging([FromQuery] ProductGetPagingRequest request)
        {
            var products = await _productService.RetrieveAll(request);
            if (products == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot get product list"));
            return Ok(CustomAPIResponse<PagedResult<ProductViewModel>>.Success(products, StatusCodes.Status200OK));
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int productId = await _productService.Create(request);
            if (productId <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create this product"));

            var product = await _productService.RetrieveById(productId);
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RetrieveById(int productId)
        {
            var product = await _productService.RetrieveById(productId);
            if (product == null)
                return NotFound(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot found this product"));
            return Ok(CustomAPIResponse<ProductViewModel>.Success(product, StatusCodes.Status200OK));
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productService.Update(request);
            if (records <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot update this product"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("delete/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int producId)
        {
            int records = await _productService.Delete(producId);
            if (records <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this product"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        ////Product Images
        [HttpGet("{productId}/images/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RetrieveImageByProductId(int productId)
        {
            var productImages = await _productImageService.RetrieveAll(new ProductImageGetPagingRequest() { ProductId = productId });
            if (productImages == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot get images of this product "));
            return Ok(CustomAPIResponse<PagedResult<ProductImageViewModel>>.Success(productImages, StatusCodes.Status200OK));
        }

        [HttpGet("images/{productImageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RetrieveImageById(int productImageId)
        {
            var productImage = await _productImageService.RetrieveById(productImageId);
            if (productImage == null)
                return NotFound(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot found this product image"));
            return Ok(CustomAPIResponse<ProductImageViewModel>.Success(productImage, StatusCodes.Status200OK));
        }

        [HttpPost("images/add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateImages([FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int count = await _productImageService.Create(request);
            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create images for this product"));
            var pagingRequest = new ProductImageGetPagingRequest() { ProductId = request.ProductId };
            var productImages = await _productImageService.RetrieveAll(pagingRequest);
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpPost("image/add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSingleImage([FromForm] ProductImageCreateSingleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int productImgId = await _productImageService.CreateSingleImage(request);
            if (productImgId <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create sub image for this product"));
            var productImage = await _productImageService.RetrieveById(productImgId);
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpPut("images/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateImage([FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productImageService.Update(request);
            if (records <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot update this product image"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("images/delete/{imageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int records = await _productImageService.Delete(imageId);
            if (records == 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this product image"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }
    }
}