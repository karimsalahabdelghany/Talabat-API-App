using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositiries
{
    public interface IbasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string BasketId);

        Task <bool> DeleteBasketAsync (string BasketId);
        
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);

            
    }
}
