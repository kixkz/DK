using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IShoppingCartRepo
    {
        public Task AddPurchaseToCart(ShoppingCart purchase);

        public Task<ShoppingCart> GetCart(int userId);

        public Task Delete(int userId);

        public Task<ShoppingCart> Update(ShoppingCart purchase, int userId);
    }
}
