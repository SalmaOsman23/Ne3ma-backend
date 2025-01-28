using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neama.Core.Entities;
using Neama.Core.Repositories;
using Neama.Dtos;
using Neama.Errors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISurpriseBagRepository _surpriseBagRepository;

        public OrdersController(IOrderRepository orderRepository, ISurpriseBagRepository surpriseBagRepository)
        {
            _orderRepository = orderRepository;
            _surpriseBagRepository = surpriseBagRepository;
        }

        [HttpPost("create")]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderCreateDto orderDto)
        {
            var user = User.Identity.Name;

            var surpriseBag = await _surpriseBagRepository.GetSurpriseBagByIdAsync(orderDto.SurpriseBagId);
            if (surpriseBag == null)
                return NotFound(new ApiResponse(404, "Surprise bag not found"));

            if (surpriseBag.QuantityAvailable < orderDto.Quantity)
                return BadRequest(new ApiResponse(400, "Not enough quantity available"));

            var order = new Order
            {
                UserId = user,
                SurpriseBagId = orderDto.SurpriseBagId,
                Quantity = orderDto.Quantity,
                PickupTime = surpriseBag.PickupTime
            };

            await _orderRepository.CreateOrderAsync(order);
            await _orderRepository.UpdateSurpriseBagQuantityAsync(orderDto.SurpriseBagId, -orderDto.Quantity);

            return Ok(new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                SurpriseBagId = order.SurpriseBagId,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                PickupTime = order.PickupTime,
                UserDisplayName = "Consumer", // Get from user service
                SupplierName = surpriseBag.Supplier.Name,
                SurpriseBagTitle = surpriseBag.Title
            });
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetUserOrders()
        {
            var user = User.Identity.Name;

            var orders = await _orderRepository.GetUserOrdersAsync(user);
            return Ok(orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                UserId = o.UserId,
                SurpriseBagId = o.SurpriseBagId,
                Quantity = o.Quantity,
                TotalPrice = o.TotalPrice,
                PickupTime = o.PickupTime,
                UserDisplayName = o.User.DisplayName,
                SupplierName = o.SurpriseBag.Supplier.Name,
                SurpriseBagTitle = o.SurpriseBag.Title
            }).ToList());
        }
    }
}
