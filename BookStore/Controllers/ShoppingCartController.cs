using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.Models.Models;
using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(AddToCart))]
        public async Task<IActionResult> AddToCart(int bookId ,int userId)
        {
            await _shoppingCartService.AddToCart(bookId ,userId);

            return Ok();           
        }

        [HttpPut(nameof(RemoveFromCart))]
        public async Task<IActionResult> RemoveFromCart(int bookId, int userId)
        {
            await _shoppingCartService.RemoveFromCart(bookId, userId);

            return Ok();
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int userId)
        {
            return Ok(await _shoppingCartService.GetContent(userId));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete(nameof(EmptyCart))]
        public async Task<IActionResult> EmptyCart(int userId)
        {
            await _shoppingCartService.EmptyCart(userId);

            return Ok();
        }

        [HttpPost(nameof(FinishPurchase))]
        public async Task<IActionResult> FinishPurchase(int userId)
        {
            await _shoppingCartService.FinishPurchase(userId);

            return Ok();
        }
    }
}
