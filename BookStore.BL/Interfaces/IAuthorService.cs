using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Interfaces
{
    public interface IAuthorService
    {
        AddAuthorResponse AddAuthor(AddAuthorRequest author);

        Author? DeleteAuthor(int authorId);

        IEnumerable<Author> GetAllAuthors();

        Author? GetByID(int id);

        AddAuthorResponse UpdateAuthor(AddAuthorRequest author, int id);
    }
}
