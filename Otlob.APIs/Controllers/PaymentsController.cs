using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.DTOs;
using Otlob.APIs.Errors;
using Otlob.Core.Entites;
using Otlob.Core.Service;
using Stripe;

namespace Otlob.APIs.Controllers
{
    //[Authorize]
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;
        const string endpointSecret = "whsec_bf05f2f05f7b1df1208f80f623166c482605333934818200b556093efcd88a16";

        public PaymentsController(IPaymentService _paymentService,
            IMapper _mapper)
        {
            paymentService = _paymentService;
            mapper = _mapper;
        }

        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId) 
        {
            var CustomerBaskt =  await paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (CustomerBaskt == null) return BadRequest(new ApiResponse(400, "There is a proplem with your Basket"));

            var MappedBasket = mapper.Map<CustomerBasket, CustomerBasketDto>(CustomerBaskt);

            return Ok(MappedBasket);
        }



        [HttpPost("webhook")]
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
                    await paymentService.UpdatePaymentIntentToSucceedOrFaild(PaymentIntent.Id, false);

                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentService.UpdatePaymentIntentToSucceedOrFaild(PaymentIntent.Id, true);

                }
                // ... handle other event types
                

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }




    }
}
