using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IBookRepository
    {
        Book AddBook(Book book);

        Book? DeleteBook(int bookId);

        IEnumerable<Book> GetAllBooks();

        Book? GetByID(int id);

        Book UpdateBook(Book book);
    }
}
