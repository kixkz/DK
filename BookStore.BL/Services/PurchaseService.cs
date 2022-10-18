using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.BL.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepo _purchaseRepo;

        public PurchaseService(IPurchaseRepo purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        public async Task<Guid> DeletePurchase(Purchase purchase)
        {
            var result = await _purchaseRepo.DeletePurchase(purchase);

            return result;
        }

        public async Task<IEnumerable<Purchase>> GetPurchase(int userId)
        {
            return await _purchaseRepo.GetPurchase(userId);
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            return await _purchaseRepo.SavePurchase(purchase);
        }
    }
}
