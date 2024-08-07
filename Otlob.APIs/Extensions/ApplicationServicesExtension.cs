using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.Errors;
using Otlob.APIs.Helpers;
using Otlob.Core;
using Otlob.Core.Repositories;
using Otlob.Core.Service;
using Otlob.Repository;
using Otlob.Service;

namespace Otlob.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this  IServiceCollection Services) 
        {

            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped (typeof( IBasketRepository),typeof(BasketRepository) ) ;
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Opetions =>
            {
                Opetions.InvalidModelStateResponseFactory = (actionContext) =>
                {



                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                               .SelectMany(P => P.Value.Errors)
                               .Select(e => e.ErrorMessage)
                               .ToArray();





                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {


                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });


            return Services; 

        }
    }
}
