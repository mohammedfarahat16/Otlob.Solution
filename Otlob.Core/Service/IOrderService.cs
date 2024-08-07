using Otlob.Core.Entites.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Service
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress);


        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);

        Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail,int OrderId);


        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
