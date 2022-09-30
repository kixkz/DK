using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.DL.Repositories.InMemotyRepositories
{
    public class BookRepository : IBookRepository
    {
        private static List<Book> _books = new List<Book>();
        public BookRepository()
        {
            _books = new List<Book>();
        }

        public Book AddBook(Book book)
        {
            try
            {
                _books.Add(book);
            }
            catch (Exception e)
            {
                return null;
            }

            return book;
        }

        public Book? DeleteBook(int bookId)
        {
            if (bookId < 0) return null;

            var book = _books.FirstOrDefault(x => x.Id == bookId);

            _books.Remove(book);

            if (book != null) return book;

            return book;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
        }

        public Book? GetByID(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public Book UpdateBook(Book book)
        {
            var existingAuthor = _books.FirstOrDefault(x => x.Id == book.Id);
            if (existingAuthor == null) return null;

            _books.Remove(existingAuthor);

            _books.Add(book);

            return book;
        }
    }
}
