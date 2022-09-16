using FurnitureWeb.Services.Catalog.CartItems;
using FurnitureWeb.Services.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemServices _cartItemService;

        public CartItemsController(ICartItemServices cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] CartItemGetPagingRequest request)
        {
            var categories = await _cartItemService.RetrieveAll(request);
            if (categories == null)
                return BadRequest();
            return Ok(categories);
        }

        [HttpGet("{cartItemId}")]
        public async Task<IActionResult> RetrieveById(int cartItemId)
        {
            var cartItem = await _cartItemService.RetrieveById(cartItemId);
            if (cartItem == null)
                return BadRequest();
            return Ok(cartItem);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] CartItemCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cartItemId = await _cartItemService.Create(request);
            if (cartItemId <= 0)
                return BadRequest();
            var cartItem = await _cartItemService.RetrieveById(cartItemId);

            return CreatedAtAction(nameof(RetrieveById), new { cartItemId = cartItemId }, cartItem);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] CartItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var count = await _cartItemService.Update(request);
            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{cartItemId}")]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            int records = await _cartItemService.Delete(cartItemId);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }
    }
}