using Otlob.Core.Entites.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(o=>o.OaymentIntentId==PaymentIntentId)
        {

        }
    }
}
