using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.DTOs;
using Otlob.APIs.Errors;
using Otlob.Core;
using Otlob.Core.Entites.Order_Aggregate;
using Otlob.Core.Service;
using StackExchange.Redis;
using System.Security.Claims;

namespace Otlob.APIs.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService _orderService
            ,IMapper _mapper)
        {
            orderService = _orderService;
            mapper = _mapper;
        }

        [ProducesResponseType(typeof(Core.Entites.Order_Aggregate.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Core.Entites.Order_Aggregate.Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var MappedAddress = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var Order = await orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

            if (Order is null) return BadRequest(new ApiResponse(400, "There Is a Proplem With Yor Order"));

            return Ok(Order);
        }



        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await orderService.GetOrdersForSpecificUserAsync(BuyerEmail);

            if (Orders is null) return NotFound(new ApiResponse(404, "there is no orders for this user"));

            var MappedOrders = mapper.Map<IReadOnlyList<Core.Entites.Order_Aggregate.Order>, IReadOnlyList<OrderToReturnDto>>(Orders);

            return Ok(MappedOrders);
        }




        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrdersByIdForSpecificUserAsync(BuyerEmail, id);

            if (order is null) return NotFound(new ApiResponse(404, $"there is no order for User Id no# {id}"));

            var MappedOrder = mapper.Map<Core.Entites.Order_Aggregate.Order, OrderToReturnDto>(order);

            return Ok(MappedOrder);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods() 
        {
            var deliveryMethods = await orderService.GetDeliveryMethodsAsync(); 

            return Ok(deliveryMethods);
        }


    }
}
