using Domain.Entities;
using FurnitureWeb.Services.Catalog.Discounts;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot get discount list"));
            return Ok(CustomAPIResponse<PagedResult<DiscountViewModel>>.Success(discounts, StatusCodes.Status200OK));
        }

        [HttpGet("{discountId}")]
        public async Task<IActionResult> RetrieveById(int discountId)
        {
            var discount = await _discountServices.RetrieveById(discountId);

            if (discount == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot found this discount"));
            return Ok(CustomAPIResponse<DiscountViewModel>.Success(discount, StatusCodes.Status200OK));
        }

        [HttpGet("aplly/{discountCode}")]
        public async Task<IActionResult> ApplyDiscount(string discountCode)
        {
            var state = await _discountServices.ApllyDiscount(discountCode);
            return Ok(CustomAPIResponse<string>.Success(state, StatusCodes.Status200OK));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] DiscountCreateRequest request)
        {
            var discountId = await _discountServices.Create(request);

            if (discountId <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create this discount"));

            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] DiscountUpdateRequest request)
        {
            var count = await _discountServices.Update(request);

            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot update this discount"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("delete/{discountId}")]
        public async Task<IActionResult> Delete(int discountId)
        {
            var count = await _discountServices.Delete(discountId);

            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this discount"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }
    }
}