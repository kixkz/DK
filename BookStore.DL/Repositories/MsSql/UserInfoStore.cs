using BookStore.Models.Models;
using System.Data.SqlClient;
using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class UserInfoStore : IUserPasswordStore<User>, IUserRoleStore<User>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserInfoRepository> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserInfoStore(IConfiguration configuration, ILogger<UserInfoRepository> logger, IPasswordHasher<User> passwordHasher)
        {
            _configuration = configuration;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    user.Password = _passwordHasher.HashPassword(user, user.Password);

                    var result = await conn.ExecuteAsync("INSERT INTO [UserInfo] (DisplayName, UserName, Email, Password, CreatedDate) VALUES (@DisplayName, @UserName, @Email, @Password, @CreatedDate)", user);

                    return IdentityResult.Success;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(CreateAsync)}:{e.Message}", e);
                return null;
            }
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
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

                    return await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM UserInfo WITH(NOLOCK) WHERE Username=@UserName ", new { Username = normalizedUserName });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(FindByNameAsync)}:{e.Message}", e);
                return null;
            }
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync(cancellationToken);

                    var result = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM UserInfo WITH(NOLOCK) WHERE UserName=@UserName ", new { UserName = user.UserName });

                    return result.UserName.Normalize();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetNormalizedUserNameAsync)}:{e.Message}", e);
                return null;
            }
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync(cancellationToken);

                    var result = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM UserInfo WITH(NOLOCK) WHERE UserId=@UserId ", new { UserId = user.UserId });

                    return result?.UserId.ToString();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetUserIdAsync)}:{e.Message}", e);
                return String.Empty;
            }
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(await GetPasswordHashAsync(user, cancellationToken));
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            await conn.OpenAsync(cancellationToken);

            await conn.ExecuteAsync("UPDATE UserInfo SET Password = @passwordHash WHERE UserId = @userId", new { user.UserId, passwordHash });
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            await conn.OpenAsync(cancellationToken);

            return await conn.QueryFirstOrDefault("SELECT Password FROM WITH(NOLOCK) WHERE UserId = @userId", new { user.UserId });
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var result = await conn.QueryAsync<string>(@"SELECT r.RoleName FROM Roles r
                                                    WHERE r.Id IN 
                                                    (SELECT ur.Id FROM UserRoles ur WHERE ur.UserId = @UserId )", new { UserId = user.UserId});

                    return result.ToList();
                }
                catch (Exception)
                {
                    _logger.LogError($"Error in {nameof(FindByNameAsync)}");
                    return null;
                }


            }
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
