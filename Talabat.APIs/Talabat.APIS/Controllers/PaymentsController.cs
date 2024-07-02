using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Models;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
    
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_aa7ce43f389710ac95333e8ecb4ff8e645dde7a0fca66e55d433dce01b04959d";

        public PaymentsController(IPaymentService paymentService,IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerbasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerbasket == null) return BadRequest(new ApiResponse(400, "There is a problem with your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket,CustomerBasketDto>(customerbasket);
            return Ok(MappedBasket);
        }

        [HttpPost("webhook")]  // post => baseurl/api/payments/webhook
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                  await  _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);
                }
                

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
