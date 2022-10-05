using BookStore.Models.Models;
using BookStore.Models.Requests;

namespace BookStore.DL.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> AddBook(Book book);

        Task<Book?> DeleteBook(int bookId);

        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book?> GetByID(int id);

        Task<Book> UpdateBook(Book book);

        Task<bool> IsAuthorWithBooks(int id);

        Task<bool> IsBookDuplicated(AddBookRequest book);
    }
}
