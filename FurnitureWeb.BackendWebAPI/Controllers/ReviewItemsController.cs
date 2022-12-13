using FurnitureWeb.Services.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewItemsController : ControllerBase
    {
        private readonly IReviewItemServices _reviewServices;

        public ReviewItemsController(IReviewItemServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RetrieveAll([FromQuery] ReviewItemGetPagingRequest request)
        {
            var reviews = await _reviewServices.RetrieveAll(request);

            if (reviews == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot get review list"));
            return Ok(CustomAPIResponse<PagedResult<ReviewItemViewModel>>.Success(reviews, StatusCodes.Status200OK));
        }

        [HttpPut("status/change")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int reviewItemId)
        {
            var count = await _reviewServices.ChangeReviewStatus(reviewItemId);

            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot change this review status"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> RetrieveById(int reviewId)
        {
            var review = await _reviewServices.RetrieveById(reviewId);

            if (review == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot found this review"));
            return Ok(CustomAPIResponse<ReviewItemViewModel>.Success(review, StatusCodes.Status200OK));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] ReviewItemCreateRequest request)
        {
            var reviewId = await _reviewServices.Create(request);

            if (reviewId <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create this review"));

            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ReviewItemUpdateRequest request)
        {
            var count = await _reviewServices.Update(request);

            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot update this review"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("delete/{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var count = await _reviewServices.Delete(reviewId);

            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this review"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }
    }
}