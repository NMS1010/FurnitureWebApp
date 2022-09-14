using FurnitureWeb.Services.Catalog.Discounts;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountServices _discountServices;

        public DiscountsController(IDiscountServices discountServices)
        {
            _discountServices = discountServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] DiscountGetPagingRequest request)
        {
            var discounts = await _discountServices.RetrieveAll(request);

            if (discounts == null)
                return BadRequest();
            return Ok(discounts);
        }

        [HttpGet("{discountId}")]
        public async Task<IActionResult> RetrieveById(int discountId)
        {
            var discount = await _discountServices.RetrieveById(discountId);

            if (discount == null)
                return BadRequest();
            return Ok(discount);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] DiscountCreateRequest request)
        {
            var discountId = await _discountServices.Create(request);

            if (discountId <= 0)
                return BadRequest();
            var discount = await _discountServices.RetrieveById(discountId);

            return CreatedAtAction(nameof(RetrieveById), new { discountId = discountId }, discount);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] DiscountUpdateRequest request)
        {
            var count = await _discountServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{discountId}")]
        public async Task<IActionResult> Delete(int discountId)
        {
            var count = await _discountServices.Delete(discountId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}