using FurnitureWeb.Services.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewItemsController : ControllerBase
    {
        private readonly IReviewItemServices _reviewServices;

        public ReviewItemsController(IReviewItemServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] ReviewItemGetPagingRequest request)
        {
            var reviews = await _reviewServices.RetrieveAll(request);

            if (reviews == null)
                return BadRequest();
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> RetrieveById(int reviewId)
        {
            var review = await _reviewServices.RetrieveById(reviewId);

            if (review == null)
                return BadRequest();
            return Ok(review);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] ReviewItemCreateRequest request)
        {
            var reviewId = await _reviewServices.Create(request);

            if (reviewId <= 0)
                return BadRequest();
            var review = await _reviewServices.RetrieveById(reviewId);

            return CreatedAtAction(nameof(RetrieveById), new { reviewId = reviewId }, review);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ReviewItemUpdateRequest request)
        {
            var count = await _reviewServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var count = await _reviewServices.Delete(reviewId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}