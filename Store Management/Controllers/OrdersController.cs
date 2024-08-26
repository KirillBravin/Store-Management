using Microsoft.AspNetCore.Mvc;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Store_Management.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromQuery] int userId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            var result = await _orderService.AddOrder(userId, productId, quantity);

            if (result)
            {
                return Ok("Order added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the order.");
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found.");
            }

            return Ok(orders);
        }

        [HttpPut("ModifyOrder")]
        public async Task<IActionResult> ModifyOrder(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            var result = await _orderService.ModifyOrder(id, order);

            if (result)
            {
                return Ok("Order modified successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while modifying the order.");
            }
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);

            if (result)
            {
                return Ok("Order successfully deleted.");
            }
            else
            {
                return NotFound("Order not found or could not be deleted.");
            }
        }
    }
}
