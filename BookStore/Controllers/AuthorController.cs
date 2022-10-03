using System.Net;
using BookStore.BL.Interfaces;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorRepository)
        {
            _logger = logger;
            _authorService = authorRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get authors")]
        public IActionResult Get()
        {
            return Ok(_authorService.GetAllAuthors());
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetById")]
        public IActionResult GetAuthorById(int id)
        {
            if (id <= 0) return BadRequest($"Parameter id: {id} mu be greater than zero !");
            var result = _authorService.GetByID(id);
            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult AddAuthor([FromBody] AddAuthorRequest authorRequest)
        {
            var result = _authorService.AddAuthor(authorRequest);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public IActionResult Update(AddAuthorRequest author, int id)
        {
            if (_authorService.GetByID(id) ==null)
            {
                return NotFound($"Author with id: {id} not exsit");
            }

            return Ok(_authorService.UpdateAuthor(author, id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _authorService.DeleteAuthor(id);
            return Ok();
        }


    }
}