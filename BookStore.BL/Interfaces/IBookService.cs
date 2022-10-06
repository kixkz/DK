using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
        Task<AddBookResponse> AddBook(AddBookRequest book);

        Task<Book?> DeleteBook(int bookId);

        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book?> GetByID(int id);

        Task<AddBookResponse> UpdateBook(UpdateBookRequest book, int id);
    }
}
