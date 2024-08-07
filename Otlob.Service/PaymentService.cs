using Microsoft.Extensions.Configuration;
using Otlob.Core;
using Otlob.Core.Entites;
using Otlob.Core.Entites.Order_Aggregate;
using Otlob.Core.Repositories;
using Otlob.Core.Service;
using Otlob.Core.Specifications.Order_Spec;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Otlob.Core.Entites.Product;

namespace Otlob.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration _configuration
            ,IBasketRepository _basketRepository
            ,IUnitOfWork _unitOfWork)
        {
            configuration = _configuration;
            basketRepository = _basketRepository;
            unitOfWork = _unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeKeys:Secretkey"];
            var Basket = await basketRepository.GetBasketAsync(BasketId);

            if (Basket is null) return null;
            decimal ShippingPrice=0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }

            if (Basket.Items.Count > 0) 
            {
                foreach (var item in Basket.Items) 
                {
                    var Product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    
                    if(item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }


            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) 
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal*100 + ShippingPrice*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };
                paymentIntent = await Service.CreateAsync(Options);

                Basket.PaymentIntentId= paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else 
            {
                var Options = new PaymentIntentUpdateOptions() 
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),


                };
                paymentIntent =await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await basketRepository.UpdateBasketAsync(Basket);
            return Basket;

        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFaild(string PaymentIntentId, bool flag)
        {
            var Spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var Order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);

            if (flag)
            {
                Order.Status = OrderStatus.PaymentReceived;
            }
            else 
            {
                Order.Status = OrderStatus.PaymentFaild;

            }

            unitOfWork.Repository<Order>().Update(Order);

            await unitOfWork.CompeleteAsync();

            return Order;


        }
    }
}
