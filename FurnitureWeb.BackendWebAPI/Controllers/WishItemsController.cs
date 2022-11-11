using FurnitureWeb.Services.Catalog.WishItems;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishItemsController : ControllerBase
    {
        private readonly IWishItemServices _wishItemServices;

        public WishItemsController(IWishItemServices wishItemServices)
        {
            _wishItemServices = wishItemServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] WishItemGetPagingRequest request)
        {
            var wishItems = await _wishItemServices.RetrieveAll(request);

            if (wishItems == null)
                return BadRequest();
            return Ok(wishItems);
        }

        [HttpGet("{wishItemId}")]
        public async Task<IActionResult> RetrieveById(int wishItemId)
        {
            var wishItem = await _wishItemServices.RetrieveById(wishItemId);

            if (wishItem == null)
                return BadRequest();
            return Ok(wishItem);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] WishItemCreateRequest request)
        {
            var wishItemId = await _wishItemServices.Create(request);

            if (wishItemId <= 0)
                return BadRequest();
            var wishItem = await _wishItemServices.RetrieveById(wishItemId);

            return CreatedAtAction(nameof(RetrieveById), new { wishItemId = wishItemId }, wishItem);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] WishItemUpdateRequest request)
        {
            var count = await _wishItemServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{wishItemId}")]
        public async Task<IActionResult> Delete(int wishItemId)
        {
            var count = await _wishItemServices.Delete(wishItemId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}