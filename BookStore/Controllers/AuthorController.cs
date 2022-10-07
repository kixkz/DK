using System.Net;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.Models.MediatR.Commands;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorRepository, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _authorService = authorRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get all authors")]
        public async Task<IActionResult> Get()
        {
            var result =await _mediator.Send(new GetAllAuthorsCommand());

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost(nameof(AddAuthorRange))]
        public async Task<IActionResult> AddAuthorRange([FromBody]AddMultipleAuthorsRequest addMultipleAuthors)
        {
            if (addMultipleAuthors != null && !addMultipleAuthors.AuthorRequest.Any())
            {
                return BadRequest(addMultipleAuthors);
            }

            var authorCollection = _mapper.Map<IEnumerable<Author>>(addMultipleAuthors.AuthorRequest);

            var result = await _mediator.Send(new AddMultipleAuthorsCommand(authorCollection));

            if (!result) return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            if (id <= 0) return BadRequest($"Parameter id: {id} must be greater than zero !");

            var result = await _mediator.Send(new GetAuthorByIdCommand(id));

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddAuthor))]
        public async Task<IActionResult> AddAuthor([FromBody] AddAuthorRequest authorRequest)
        {
            var result = await _mediator.Send(new AddAuthorCommand (authorRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuthorRequest author)
        {
            return Ok(await _authorService.UpdateAuthor(author, author.Id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _mediator.Send(new GetAuthorByIdCommand(id)) == null)
            {
                return BadRequest("Author not exist");
            }

            var result = await _mediator.Send(new DeleteAuthorCommand (id));

            return Ok(result);
        }


    }
}