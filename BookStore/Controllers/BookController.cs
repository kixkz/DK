using System.Net;
using BookStore.BL.Interfaces;
using BookStore.BL.Kafka;
using BookStore.BL.Services;
using BookStore.Cash.Models;
using BookStore.Models.MediatR.Commands;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMediator _mediator;
        private readonly Consumer<int, Book> _consumer;
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, IBookService bookRepository, IMediator mediator, Consumer<int, Book> consumer)
        {
            _logger = logger;
            _bookService = bookRepository;
            _mediator = mediator;
            _consumer = consumer;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get all books")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllBooksCommand()));
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest($"Parameter id: {id} mu be greater than zero !");
            
            var result = await _mediator.Send(new GetBookByIdCommand(id));

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequest addBookRequest)
        {
            var result = await _mediator.Send(new AddBookCommand(addBookRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest book)
        {
            var result = await _mediator.Send(new UpdateBookCommand(book));

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _bookService.GetByID(id) == null)
            {
                return BadRequest("Book not exist");
            }

            var result = await _mediator.Send(new DeleteBookCommand(id));

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Kafka")]
        public async Task<IActionResult> GetCash()
        {
            return Ok(_consumer._data.Count());
        }
    }
}