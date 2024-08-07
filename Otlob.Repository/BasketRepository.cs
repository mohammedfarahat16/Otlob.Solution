﻿using Otlob.Core.Entites;
using Otlob.Core.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otlob.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;   
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
            
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await database.StringGetAsync(BasketId);

            //if (Basket.IsNull) return null;
            //else
            //    return JsonSerializer.Deserialize<CustomerBasket>(Basket);

            return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);
            var CreatedOrUpdated =  await database.StringSetAsync(Basket.Id, JsonBasket,TimeSpan.FromDays(1));

            if (!CreatedOrUpdated) return null;


            return await GetBasketAsync(Basket.Id);

        }
    }
}
