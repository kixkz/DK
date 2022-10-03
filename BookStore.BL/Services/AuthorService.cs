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
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public AddAuthorResponse AddAuthor(AddAuthorRequest authorRequest)
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
                Author = result
            };
        }

        public Author? DeleteAuthor(int authorId)
        {
            return _authorRepository.DeleteAuthor(authorId);
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authorRepository.GetAllAuthors();
        }

        public Author? GetByID(int id)
        {
            //try
            //{
            //    throw new ArgumentNullException("Test");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    return null;
            //}
            return _authorRepository.GetByID(id);
        }

        public AddAuthorResponse UpdateAuthor(AddAuthorRequest addAuthorRequest, int id)
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
                Author = result
            };
        }
    }
}
