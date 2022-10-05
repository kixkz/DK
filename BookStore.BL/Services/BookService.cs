using System.Net;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;
using Microsoft.Extensions.Logging;

namespace BookStore.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public async Task<AddBookResponse> AddBook(AddBookRequest addBookRequest)
        {
            var book = _mapper.Map<Book>(addBookRequest);
            var result = await _bookRepository.AddBook(book);

            return new AddBookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }

        public async Task<Book?> DeleteBook(int bookId)
        {
            return await _bookRepository.DeleteBook(bookId);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBooks();
        }

        public async Task<Book?> GetByID(int id)
        {
            return await _bookRepository.GetByID(id);
        }

        public async Task<bool> IsBookDuplicated(AddBookRequest book)
        {
            return await _bookRepository.IsBookDuplicated(book);
        }

        public async Task<AddBookResponse> UpdateBook(AddBookRequest addBookRequest, int id)
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
                Book = await result
            };
        }
    }
}
