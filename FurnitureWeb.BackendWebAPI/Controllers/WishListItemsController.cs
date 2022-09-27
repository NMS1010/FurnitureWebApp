using FurnitureWeb.Services.Catalog.WishListItems;
using FurnitureWeb.ViewModels.Catalog.WishListItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListItemsController : ControllerBase
    {
        private readonly IWishListItemServices _wishListItemServices;

        public WishListItemsController(IWishListItemServices wishListItemServices)
        {
            _wishListItemServices = wishListItemServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] WishListItemGetPagingRequest request)
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
        public async Task<IActionResult> Create([FromForm] WishListItemCreateRequest request)
        {
            var wishListItemId = await _wishListItemServices.Create(request);

            if (wishListItemId <= 0)
                return BadRequest();
            var wishListItem = await _wishListItemServices.RetrieveById(wishListItemId);

            return CreatedAtAction(nameof(RetrieveById), new { wishListItemId = wishListItemId }, wishListItem);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] WishListItemUpdateRequest request)
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