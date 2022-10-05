using System.Net;
using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, IBookService bookRepository)
        {
            _logger = logger;
            _bookService = bookRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get all books")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bookService.GetAllBooks());
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest($"Parameter id: {id} mu be greater than zero !");
            var result = await _bookService.GetByID(id);
            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequest addBookRequest)
        {
            var result = await _bookService.AddBook(addBookRequest);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> Update(AddBookRequest book, int id)
        {
            if (await _bookService.GetByID(id) == null)
            {
                return NotFound($"Book with id: {id} not exsit");
            }

            return Ok( _bookService.UpdateBook(book, id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteBook(id);
            return Ok(result);
        }
    }
}