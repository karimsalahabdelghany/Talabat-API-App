using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Services;
using Talabat.Services;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService oredrService, IMapper mapper)
        {
            _orderService = oredrService;
            _mapper = mapper;
            
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Authorize]        //Create Order
        [HttpPost]        //POST => baseurl/api/Orders
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var Order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

            if (Order is null) return BadRequest(new ApiResponse(400, "There is a Problem With Your Order"));
            return Ok(Order);

        }
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]     //Get => baseurl/api/Orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There is No Orders For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList< OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{orderid}")]     //Get => BaseUrl/api/Orders/1
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int orderid)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, orderid);
            if (Order is null) return NotFound(new ApiResponse(404, $"Order With {orderid} id is Not Found"));
            var MappedOrder =_mapper.Map<Order,OrderToReturnDto>(Order);
            return Ok(MappedOrder);
        }

        [HttpGet("DeliveryMethods")]   //Get => BaseUrl/api/Orders/DeliveryMethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>>GetDeliveryMethods()
        {
            //var DeliveryMethods =await _unitOfWork.Repository<Order>().GetAllAsync();
            //return Ok(DeliveryMethods);
            var DeliveryMethods =await _orderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }
    }
}
