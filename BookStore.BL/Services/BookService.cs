using System.Net;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories.InMemotyRepositories;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public AddBookResponse AddBook(AddBookRequest addBookRequest)
        {
            var book = _mapper.Map<Book>(addBookRequest);
            var result = _bookRepository.AddBook(book);

            return new AddBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }

        public Book? DeleteBook(int bookId)
        {
            return _bookRepository.DeleteBook(bookId);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book? GetByID(int id)
        {
            return _bookRepository.GetByID(id);
        }

        public AddBookResponse UpdateBook(AddBookRequest addBookRequest, int id)
        {
            var auth = _bookRepository.GetByID(id);
            if (auth == null)
                return new AddBookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Book not exist"
                };
            var book = _mapper.Map<Book>(addBookRequest);
            book.Id = id;
            var result = _bookRepository.UpdateBook(book);
            return new AddBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }
    }
}
