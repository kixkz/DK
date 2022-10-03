using BookStore.BL.Interfaces;
using BookStore.DL.Repositories.InMemotyRepositories;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorRepository;
        private static readonly List<Author> _authors = new List<Author>();


        private readonly ILogger<Author> _logger;

        public AuthorController(ILogger<Author> logger, IAuthorService authorRepository)
        {
            _logger = logger;
            _authorRepository = authorRepository;
        }

        [HttpGet("Get authors")]
        public IEnumerable<Author> Get()
        {
            return _authorRepository.GetAllAuthors();
        }

        [HttpGet("GetById")]
        public Author GetById(int id)
        {
            return _authorRepository.GetByID(id);
        }

        [HttpPost]
        public void Add([FromBody] Author model)
        {
            _authorRepository.AddAuthor(model);
        }

        [HttpPut]
        public void Update(Author author)
        {
            _authorRepository.UpdateAuthor(author);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _authorRepository.DeleteAuthor(id);
        }


    }
}