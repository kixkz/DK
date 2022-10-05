using System.Data.SqlClient;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class PersonSqlRepository : IPersonRepository
    {
        private readonly ILogger<PersonSqlRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonSqlRepository(ILogger<PersonSqlRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Person> AddUser(Person user)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Person] (Name, Age, DateOfBirth) VALUES (@Name, @Age, @DateOfBirth, @NickName)", user);

                    return user;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddUser)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Person?> DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> UpdateUser(Person user)
        {
            throw new NotImplementedException();
        }
    }
}
