using System;
using BookStore.DL.Interfaces;
using BookStore.Models.Configurations;
using BookStore.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookStore.DL.Repositories.MongoRepo
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly IOptions<MongoDbConfiguration> _mongoSettings;
        private readonly IMongoCollection<ShoppingCart> _purchaseCollection;

        public ShoppingCartRepo(IOptions<MongoDbConfiguration> mongoSettings)
        {
            _mongoSettings = mongoSettings;
            MongoClient dbClient = new MongoClient(_mongoSettings.Value.ConnectionString);
            var database = dbClient.GetDatabase(_mongoSettings.Value.DatabaseName);
            _purchaseCollection = database.GetCollection<ShoppingCart>(nameof(ShoppingCart));
        }

        public async Task AddPurchaseToCart(ShoppingCart purchase)
        {
            await _purchaseCollection.InsertOneAsync(purchase);
        }

        public async Task Delete(int userId)
        {
            await _purchaseCollection.DeleteOneAsync(x => x.UserId == userId);
        }

        public async Task<ShoppingCart> GetCart(int userId)
        {
            var ressult = await _purchaseCollection.FindAsync(x => x.UserId == userId);

            return ressult.FirstOrDefault();
        }

        public async Task<ShoppingCart> Update(ShoppingCart purchase, int userId)
        {
            var update = Builders<ShoppingCart>.Update.Set(p => p.Books, purchase.Books);

            return await _purchaseCollection.FindOneAndUpdateAsync(x => x.UserId == userId, update);
        }
    }
}
