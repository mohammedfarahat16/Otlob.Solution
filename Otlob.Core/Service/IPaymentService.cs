using Otlob.Core.Entites;
using Otlob.Core.Entites.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Service
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);

        Task<Order> UpdatePaymentIntentToSucceedOrFaild(string PaymentIntentId, bool flag);


    }
}
