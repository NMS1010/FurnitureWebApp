using FurnitureWeb.Services.Catalog.Orders;
using FurnitureWeb.ViewModels.Catalog.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] OrderGetPagingRequest request)
        {
            var orders = await _orderServices.RetrieveAll(request);

            if (orders == null)
                return BadRequest();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> RetrieveById(int orderId)
        {
            var order = await _orderServices.RetrieveById(orderId);

            if (order == null)
                return BadRequest();
            return Ok(order);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] OrderCreateRequest request)
        {
            var orderId = await _orderServices.Create(request);

            if (orderId <= 0)
                return BadRequest();
            var order = await _orderServices.RetrieveById(orderId);

            return CreatedAtAction(nameof(RetrieveById), new { orderId = orderId }, order);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] OrderUpdateRequest request)
        {
            var count = await _orderServices.Update(request);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            var count = await _orderServices.Delete(orderId);

            if (count <= 0)
                return BadRequest();
            return Ok();
        }
    }
}