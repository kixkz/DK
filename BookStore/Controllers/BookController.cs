using BookStore.BL.Interfaces;
using BookStore.DL.Repositories.InMemotyRepositories;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookRepository;
        private static readonly List<Book> _authors = new List<Book>();


        private readonly ILogger<Person> _logger;

        public BookController(ILogger<Person> logger, IBookService bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        [HttpGet("Get books")]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAllBooks();
        }

        [HttpGet("Get")]
        public Book GetById(int id)
        {
            return _bookRepository.GetByID(id);
        }

        [HttpPost]
        public void Add([FromBody] Book model)
        {
            _bookRepository.AddBook(model);
        }

        [HttpPut]
        public void Update(Book book)
        {
            _bookRepository.UpdateBook(book);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _bookRepository.DeleteBook(id);
        }


    }
}