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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILogger<AuthorService> logger, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
            _bookRepository = bookRepository;
        }

        public async Task<AddAuthorResponse> AddAuthor(AddAuthorRequest authorRequest)
        {
            var auth = _authorRepository.GetAuthorByName(authorRequest.Name);

            if (auth != null)
                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exist"
                };

            var author = _mapper.Map<Author>(authorRequest);
            var result = _authorRepository.AddAuthor(author);

            return new AddAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = await result
            };
        }

        public async Task<Author?> DeleteAuthor(int authorId)
        {
            if (await _bookRepository.IsAuthorWithBooks(authorId))
            {
                return null;
            }

            return await _authorRepository.DeleteAuthor(authorId);
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAllAuthors();
        }

        public async Task<Author?> GetByID(int id)
        {
            return await _authorRepository.GetByID(id);
        }

        public async Task<AddAuthorResponse> UpdateAuthor(AddAuthorRequest addAuthorRequest, int id)
        {
            var auth = _authorRepository.GetAuthorByName(addAuthorRequest.Name);
            if (auth == null)
                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author not exist"
                };
            var author = _mapper.Map<Author>(addAuthorRequest);
            author.Id = id;
            var result = _authorRepository.UpdateAuthor(author);
            return new AddAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = await result
            };
        }

        public async Task<bool> AddMultipleAuthors(IEnumerable<Author> authosCollection)
        {
            return await _authorRepository.AddMultipleAuthors(authosCollection);
        }
    }
}
