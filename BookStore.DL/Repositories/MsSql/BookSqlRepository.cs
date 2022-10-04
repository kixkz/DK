using System.Data.SqlClient;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class BookSqlRepository : IBookRepository
    {
        private readonly ILogger<AuthorSqlRepository> _logger;
        private readonly IConfiguration _configuration;

        public BookSqlRepository(ILogger<AuthorSqlRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Book> AddBook(Book book)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Books] (AuthorId, Title, LastUpdated, Quantity, Price) VALUES(@AuthorId, @Title, GETDATE(), @Quantity, @Price)", book);

                    return book;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddBook)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Book?> DeleteBook(int bookId)
        {
            var deletedBook = await GetByID(bookId);

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();


                    var result = await conn.ExecuteAsync("DELETE FROM Books WHERE Id=@id", new { id = bookId });

                    return deletedBook;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteBook)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Books WITH(NOLOCK)";

                    await conn.OpenAsync();

                    return await conn.QueryAsync<Book>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllBooks)}:{e.Message}", e);
            }

            return Enumerable.Empty<Book>();
        }

        public async Task<Book?> GetByID(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books WITH(NOLOCK) WHERE Id = @Id", new { Id = id });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetByID)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    await conn.QueryFirstOrDefaultAsync<Book>("UPDATE Books SET AuthorId=@authorid, Title=@title, LastUpdated=GETDATE(), Quantity=@quantity, Price=@price WHERE Id=@id", 
                                new { id = book.Id, authorid = book.AuthorId, title = book.Title, quantity = book.Quantity, price = book.Price });
                    return book;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateBook)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<bool> IsAuthorWithBooks(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books  WITH(NOLOCK) WHERE AuthorId = @authorid", new { authorid = id});

                    if (result != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(IsAuthorWithBooks)}:{e.Message}", e);
            }

            return false;
        }
    }
}
