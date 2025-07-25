using System.Formats.Asn1;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMarketAPI.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var orders= await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }


        // GET: api/orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task <IActionResult> GetByUserId(string userId)
        {
            var orders= await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            var order= await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);

        }

        // POST: api/orders
        [HttpPost]

        public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
        {
            await _orderService.CreateOrderAsync(createOrderDto);
            return Ok();
        }

        [HttpGet("my-orders")]
        [Authorize]
        public async Task <IActionResult> GetMyOrders()
        {
            var userId=User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task <IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }












    }
}
