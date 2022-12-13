using FurnitureWeb.Services.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemServices _orderItemService;

        public OrderItemsController(IOrderItemServices orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] OrderItemGetPagingRequest request)
        {
            var categories = await _orderItemService.RetrieveAll(request);
            if (categories == null)
                return BadRequest();
            return Ok(categories);
        }

        [HttpGet("{orderItemId}")]
        public async Task<IActionResult> RetrieveById(int orderItemId)
        {
            var orderItem = await _orderItemService.RetrieveById(orderItemId);
            if (orderItem == null)
                return BadRequest();
            return Ok(orderItem);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] OrderItemCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var orderItemId = await _orderItemService.Create(request);
            if (orderItemId <= 0)
                return BadRequest();
            var orderItem = await _orderItemService.RetrieveById(orderItemId);

            return CreatedAtAction(nameof(RetrieveById), new { orderItemId = orderItemId }, orderItem);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] OrderItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var count = await _orderItemService.Update(request);
            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{orderItemId}")]
        public async Task<IActionResult> Delete(int orderItemId)
        {
            int records = await _orderItemService.Delete(orderItemId);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }
    }
}