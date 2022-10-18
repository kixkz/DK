using System;
using BookStore.DL.Interfaces;
using BookStore.Models.Configurations;
using BookStore.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookStore.DL.Repositories.MongoRepo
{
    public class PurchaseRepo : IPurchaseRepo
    {
        private readonly IOptions<MongoDbConfiguration> _mongoSettings;
        private readonly IMongoCollection<Purchase> _purchaseCollection;

        public PurchaseRepo(IOptions<MongoDbConfiguration> mongoSettings)
        {
            _mongoSettings = mongoSettings;
            MongoClient dbClient = new MongoClient(_mongoSettings.Value.ConnectionString);
            var database = dbClient.GetDatabase(_mongoSettings.Value.DatabaseName);
            _purchaseCollection = database.GetCollection<Purchase>(nameof(Purchase));
            
        }

        public async Task<Guid> DeletePurchase(Purchase purchase)
        {
            var result = await _purchaseCollection.DeleteOneAsync(x => x.Id == purchase.Id);

            return purchase.Id;
        }

        public async Task<IEnumerable<Purchase>> GetPurchase(int userId)
        {
            var result = await _purchaseCollection.FindAsync(x => x.UserId == userId);

            return result.ToList();
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            await _purchaseCollection.InsertOneAsync(purchase);

            return purchase;
        }
    }
}
