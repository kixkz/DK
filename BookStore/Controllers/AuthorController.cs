using BookStore.DL.Repositories.InMemotyRepositories;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private static readonly List<Author> _authors = new List<Author>();


        private readonly ILogger<User> _logger;

        public AuthorController(ILogger<User> logger, IAuthorRepository authorRepository)
        {
            _logger = logger;
            _authorRepository = authorRepository;
        }

        [HttpGet(nameof(Get))]
        public IEnumerable<Author> Get()
        {
            return _authorRepository.GetAllAuthors();
        }

        [HttpGet(nameof(GetById))]
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