using BookStore.Models.Models;
using System.Data.SqlClient;
using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class UserInfoStore : IUserPasswordStore<User>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserInfoRepository> _logger;

        public UserInfoStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Authors] (UserId, DisplayName, UserName, Email, Password, CreatedDate) VALUES (@UserId, @DisplayName, @UserName, @Email, @Password, @CreatedDate)", user);

                    return IdentityResult.Success;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(CreateAsync)}:{e.Message}", e);
            }

            return null;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM UserInfo WHERE Username=@UserName WITH(NOLOCK)", new { Name = normalizedUserName });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(FindByNameAsync)}:{e.Message}", e);
            }

            return null;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
