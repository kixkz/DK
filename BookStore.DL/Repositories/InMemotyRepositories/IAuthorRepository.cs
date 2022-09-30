using BookStore.Models.Models;

namespace BookStore.DL.Repositories.InMemotyRepositories
{
    public interface IAuthorRepository
    {
        Author AddAuthor(Author author);

        Author? DeleteAuthor(int authorId);

        IEnumerable<Author> GetAllAuthors();

        Author? GetByID(int id);

        Author UpdateAuthor(Author author);
    }
}
