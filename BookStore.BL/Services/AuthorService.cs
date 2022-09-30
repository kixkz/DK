using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.BL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Author AddAuthor(Author author)
        {
            return _authorRepository.AddAuthor(author);
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
            return _authorRepository.GetByID(id);
        }

        public Author UpdateAuthor(Author author)
        {
            return _authorRepository.UpdateAuthor(author);
        }
    }
}
