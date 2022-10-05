using System.Data.SqlClient;
using System.Xml.Linq;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class AuthorSqlRepository : IAuthorRepository
    {
        private readonly ILogger<AuthorSqlRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuthorSqlRepository(ILogger<AuthorSqlRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Author> AddAuthor(Author author)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Authors] (Name, Age, DateOfBirth, NickName) VALUES(@Name, @Age, @DateOfBirth, @NickName)", author);
                    
                    return author;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddAuthor)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<bool> AddMultipleAuthors(IEnumerable<Author> authosCollection)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Authors] (Id, Name, Age, DateOfBirth, NickName) VALUES (@Name, @Age, @DateOfBirth, @NickName)", authosCollection);
                    return result > 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddMultipleAuthors)}:{e.Message}", e);
            }

            return false;
        }

        public async Task<Author?> DeleteAuthor(int authorId)
        {
            var deletedAuthor = await GetByID(authorId);

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                   
                    var result = await conn.ExecuteAsync("DELETE FROM Authors WHERE Id=@id", new {id = authorId});
                    
                    return deletedAuthor;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteAuthor)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Authors WITH(NOLOCK)";

                    await conn.OpenAsync();

                    return await conn.QueryAsync<Author>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllAuthors)}:{e.Message}", e);
            }

            return Enumerable.Empty<Author>();
        }

        public async Task<Author?> GetAuthorByName(string authorName)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Author>("SELECT * FROM Authors WHERE Name = @Name WITH(NOLOCK)", new { Name = authorName });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAuthorByName)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Author?> GetByID(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Author>("SELECT * FROM Authors WITH(NOLOCK) WHERE Id = @Id", new {Id = id});

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetByID)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Author> UpdateAuthor(Author author)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    await conn.QueryFirstOrDefaultAsync<Author>("UPDATE Authors SET Name=@name, Age=@age, DateOfBirth=@dateofbirth, NickName=@nickname WHERE Id=@id", 
                               new { id = author.Id, name = author.Name, age = author.Age, dateofbirth = author.DateOfBirth, nickname = author.NickName });
                    return author;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateAuthor)}:{e.Message}", e);
            }

            return null;
        }
    }
}
