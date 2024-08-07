using Otlob.Core.Entites;
using Otlob.Core.Entites.Order_Aggregate;
using Otlob.Core.Repositories;
using Otlob.Core.Specifications.Order_Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;

        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderService(IBasketRepository _basketRepository,
            IUnitOfWork _unitOfWork
            ,IPaymentService _paymentService)

        {
            basketRepository = _basketRepository;
            unitOfWork = _unitOfWork;
            paymentService = _paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Basket = await basketRepository.GetBasketAsync(basketId);

            var OrderItems = new List<OrderItem>();

            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var ProductItemOredered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);

                    var OrderItem = new OrderItem(ProductItemOredered, Product.Price, item.Quantity);

                    OrderItems.Add(OrderItem);

                }


            }
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);

            var ExOrder = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);

            if (ExOrder is not null) 
            {
                unitOfWork.Repository<Order>().Delete(ExOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basketId);
            }




            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);


            await unitOfWork.Repository<Order>().Add(Order);
            /////////////////

            var result =await unitOfWork.CompeleteAsync();

            if (result<0)
            {
                return null;
            }
            return Order; 

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;

        }

        public async Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(buyerEmail, OrderId);
            var Order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            return Order ;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);

            var Orders =await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return Orders; 
        }










    }
}
