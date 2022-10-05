using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<Author> AddAuthor(Author author);

        public Task<Author?> DeleteAuthor(int authorId);

        public Task<IEnumerable<Author>> GetAllAuthors();

        public Task<Author?> GetByID(int id);

        public Task<Author> UpdateAuthor(Author author);

        public Task<Author?> GetAuthorByName(string authorName);

        public Task<bool> AddMultipleAuthors(IEnumerable<Author> authosCollection);
    }
}
