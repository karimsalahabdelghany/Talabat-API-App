using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;

namespace Talabat.Repositiory
{
    public class BasketRepositiory : IbasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepositiory(IConnectionMultiplexer redis) //ASk ClR for creating object from class implement interface IConnectionMultiplexer 
        {
           _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
          return await  _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket =await _database.StringGetAsync(BasketId);
            //if (Basket.IsNull) return null;
            //else
            //    return JsonSerializer.Deserialize<CustomerBasket>(Basket); 
            return Basket.IsNull? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);


        }
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var jsonBasket =JsonSerializer.Serialize(Basket);
            var UpdatedorCreated =  await _database.StringSetAsync(Basket.Id,jsonBasket,TimeSpan.FromDays(1));
            if (!UpdatedorCreated) return null;
            else
              return await GetBasketAsync(Basket.Id);
        }
        
    }
}
