using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;

namespace Talabat.APIS.Controllers
{
    
    public class BasketsController : APIBaseController
    {
        private readonly IbasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IbasketRepository basketRepository,IMapper mapper) 
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        //Get or Recraete Basket
        [HttpGet]
        public async Task< ActionResult<CustomerBasket>>GetBasket(string basketId)
        {
          var basket = await  _basketRepository.GetBasketAsync(basketId);
            //if (basket is null) return new CustomerBasket(basketId);
            //return Ok(basket);
            return basket is null ? new CustomerBasket(basketId) : Ok(basket);
        }
        //Update or Create new Basket for first time
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasketDto Basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);
            var UpdatedorCreadtedbasket =  await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (UpdatedorCreadtedbasket is null) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedorCreadtedbasket);
        }
        //Delete Basket
        [HttpDelete("{BasketId}")]
        public async Task<ActionResult<bool>>DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
