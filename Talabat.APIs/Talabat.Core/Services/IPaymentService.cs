using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        // Func to create or update paymentIntent
        Task<CustomerBasket?>CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentintenid, bool flag);
    }
}
