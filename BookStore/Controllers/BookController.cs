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
        [HttpGet("Get books")]
        public IActionResult Get()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if (id <= 0) return BadRequest($"Parameter id: {id} mu be greater than zero !");
            var result = _bookService.GetByID(id);
            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult AddBook([FromBody] AddBookRequest addBookRequest)
        {
            var result = _bookService.AddBook(addBookRequest);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            _bookService.AddBook(addBookRequest);

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public IActionResult Update(AddBookRequest book, int id)
        {
            if (_bookService.GetByID(id) == null)
            {
                return NotFound($"Book with id: {id} not exsit");
            }

            return Ok(_bookService.UpdateBook(book, id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return Ok();
        }


    }
}