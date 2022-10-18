using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepo _shoppingCartRepo;
        private readonly IPurchaseService _purchaseService;
        private readonly IBookService _bookService;

        public ShoppingCartService(IShoppingCartRepo shoppingCartRepo, IPurchaseService purchaseService, IBookService bookService)
        {
            _shoppingCartRepo = shoppingCartRepo;
            _purchaseService = purchaseService;
            _bookService = bookService;
        }

        public async Task AddToCart(int bookId, int userId)
        {
            var bookToAdd = await _bookService.GetByID(bookId);
            var shoppingCart = await _shoppingCartRepo.GetCart(userId);

            if (bookToAdd == null)
            {
                return;
            }

            if (shoppingCart != null)
            {
                var books = shoppingCart.Books.ToList();
                books.Add(bookToAdd);
                shoppingCart.Books = books;
                await _shoppingCartRepo.Update(shoppingCart, userId);
                return;
            }

            await _shoppingCartRepo.AddPurchaseToCart(new ShoppingCart()
            {
                Id = new Guid(),
                Books = new List<Book>() { bookToAdd},
                UserId = userId
            });
        }

        public async Task EmptyCart(int userId)
        {
            var shoppingCart = await _shoppingCartRepo.GetCart(userId);

            if (shoppingCart != null)
            {
                await _shoppingCartRepo.Delete(userId);
            }
        }

        public async Task FinishPurchase(int userId)
        {
            var shoppingCart = await _shoppingCartRepo.GetCart(userId);

            if (shoppingCart != null)
            {
                var purchase = new Purchase()
                {
                    UserId = userId,
                    Books = shoppingCart.Books,
                    TotalMoney = shoppingCart.Books.Sum(x => x.Price)
                };

                await _purchaseService.SavePurchase(purchase);
                await EmptyCart(userId);
            }
        }

        public async Task<IEnumerable<Book>> GetContent(int userId)
        {
            var shoppingCart = await _shoppingCartRepo.GetCart(userId);

            if (shoppingCart != null)
            {
                return shoppingCart.Books;
            }

            return Enumerable.Empty<Book>();
        }

        public async Task RemoveFromCart(int bookId, int userId)
        {
            var bookToAdd = await _bookService.GetByID(bookId);
            var shoppingCart = await _shoppingCartRepo.GetCart(userId);

            if (shoppingCart == null)
            {
                return;
            }

            if (shoppingCart != null && shoppingCart.Books.Any(x => x.Id == bookId))
            {
                if (shoppingCart.Books.Count() == 1)
                {
                    await _shoppingCartRepo.Delete(userId);
                    return;
                }

                var books = shoppingCart.Books.ToList();
                books.Remove(books.FirstOrDefault(x => x.Id == bookId));
                shoppingCart.Books = books;
                await _shoppingCartRepo.Update(shoppingCart, userId);
            }
        }
    }
}
