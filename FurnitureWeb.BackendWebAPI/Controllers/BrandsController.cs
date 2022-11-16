using FurnitureWeb.Services.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandServices _brandServices;

        public BrandsController(IBrandServices brandServices)
        {
            _brandServices = brandServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] BrandGetPagingRequest request)
        {
            var brands = await _brandServices.RetrieveAll(request);

            if (brands == null)
                return BadRequest();
            return Ok(brands);
        }

        [HttpGet("{brandId}")]
        public async Task<IActionResult> RetrieveById(int brandId)
        {
            var brand = await _brandServices.RetrieveById(brandId);

            if (brand == null)
                return BadRequest();
            return Ok(brand);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] BrandCreateRequest request)
        {
            var brandId = await _brandServices.Create(request);

            if (brandId <= 0)
                return BadRequest();
            var brand = await _brandServices.RetrieveById(brandId);

            return CreatedAtAction(nameof(RetrieveById), new { brandId = brandId }, brand);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] BrandUpdateRequest request)
        {
            var count = await _brandServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{brandId}")]
        public async Task<IActionResult> Delete(int brandId)
        {
            var count = await _brandServices.Delete(brandId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}