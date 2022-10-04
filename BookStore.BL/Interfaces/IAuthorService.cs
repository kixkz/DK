using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Interfaces
{
    public interface IAuthorService
    {
        Task<AddAuthorResponse> AddAuthor(AddAuthorRequest author);

        Task<Author?> DeleteAuthor(int authorId);

        public Task<IEnumerable<Author>> GetAllAuthors();

        Task<Author?> GetByID(int id);

        Task<AddAuthorResponse> UpdateAuthor(AddAuthorRequest author, int id);

        Task<bool> AddMultipleAuthors(IEnumerable<Author> authosCollection);
    }
}
