using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.DTOs;
using Otlob.APIs.Errors;
using Otlob.Core.Entites;
using Otlob.Core.Repositories;

namespace Otlob.APIs.Controllers
{

    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(
            IBasketRepository _basketRepository,
            IMapper _mapper)
        {
            basketRepository = _basketRepository;
            mapper = _mapper;
        }
        //Get Or Reacreate 

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Basket = await basketRepository.GetBasketAsync(BasketId);
            if(Basket == null) 
            {
                return new CustomerBasket(BasketId);
            }
            else 
                return Basket;
        }

        //Create or update

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket) 
        {
            var MappedBasket = mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var CreatedOrUpdatedBasket  = await basketRepository.UpdateBasketAsync(MappedBasket);

            if(CreatedOrUpdatedBasket is null) return  BadRequest(new ApiResponse(400)) ;

            return Ok(CreatedOrUpdatedBasket);
        }


        //Delete Basket


        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id) 
        {
            return await basketRepository.DeleteBasketAsync(id);
        }



    }
}
