using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Secifications.OrderSpec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentintentid)
            :base(P=>P.PaymentIntentId == paymentintentid)
        { 

        }
    }
}
