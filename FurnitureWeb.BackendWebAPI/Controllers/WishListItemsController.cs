using FurnitureWeb.Services.Catalog.WishItems;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListItemsController : ControllerBase
    {
        private readonly IWishItemServices _wishListItemServices;

        public WishListItemsController(IWishItemServices wishListItemServices)
        {
            _wishListItemServices = wishListItemServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] WishItemGetPagingRequest request)
        {
            var wishListItems = await _wishListItemServices.RetrieveAll(request);

            if (wishListItems == null)
                return BadRequest();
            return Ok(wishListItems);
        }

        [HttpGet("{wishListItemId}")]
        public async Task<IActionResult> RetrieveById(int wishListItemId)
        {
            var wishListItem = await _wishListItemServices.RetrieveById(wishListItemId);

            if (wishListItem == null)
                return BadRequest();
            return Ok(wishListItem);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] WishItemCreateRequest request)
        {
            var wishListItemId = await _wishListItemServices.Create(request);

            if (wishListItemId <= 0)
                return BadRequest();
            var wishListItem = await _wishListItemServices.RetrieveById(wishListItemId);

            return CreatedAtAction(nameof(RetrieveById), new { wishListItemId = wishListItemId }, wishListItem);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] WishItemUpdateRequest request)
        {
            var count = await _wishListItemServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{wishListItemId}")]
        public async Task<IActionResult> Delete(int wishListItemId)
        {
            var count = await _wishListItemServices.Delete(wishListItemId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}