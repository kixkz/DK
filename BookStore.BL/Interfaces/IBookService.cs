using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
        AddBookResponse AddBook(AddBookRequest book);

        Book? DeleteBook(int bookId);

        IEnumerable<Book> GetAllBooks();

        Book? GetByID(int id);

        AddBookResponse UpdateBook(AddBookRequest book, int id);
    }
}
