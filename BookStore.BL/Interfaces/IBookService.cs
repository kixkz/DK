using BookStore.Models.Models;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
        Book AddBook(Book book);

        Book? DeleteBook(int bookId);

        IEnumerable<Book> GetAllBooks();

        Book? GetByID(int id);

        Book UpdateBook(Book book);
    }
}
