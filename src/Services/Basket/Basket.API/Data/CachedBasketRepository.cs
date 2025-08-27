using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
// Các chú thích trong file này viết bằng tiếng Việt
namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {

        public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellation = default)
        {
            // Try to get from cache first
            var cachedBasket = await cache.GetStringAsync(userName, cancellation);
            if (cachedBasket is not null)
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
            }
            // If not found in cache, get from repository
            var basket = await repository.GetBasket(userName, cancellation);
            if (basket is not null) 
            {
                // Store in cache for future requests
                var serializedBasket = JsonSerializer.Serialize(basket);
                // Cache with an absolute expiration of 1 hour and sliding expiration of 30 minutes
                await cache.SetStringAsync(userName, serializedBasket, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Cache will expire in 1 hour
                    SlidingExpiration = TimeSpan.FromMinutes(30) // Cache will be refreshed if accessed within 30 minutes
                }, cancellation);
            }
            ;
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellation = default)
        {
            // Store in repository
            await repository.StoreBasket(basket, cancellation);
            // Update cache
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Cache will expire in 1 hour
                SlidingExpiration = TimeSpan.FromMinutes(30) // Cache will be refreshed if accessed within 30 minutes
            }, cancellation);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellation = default)
        {
            await repository.DeleteBasket(userName, cancellation);
            await cache.RemoveAsync(userName, cancellation);
            return true;
        }

    }
}
