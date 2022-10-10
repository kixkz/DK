using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BL.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateAsync(User User);

        Task<User> CheckUserAndPass(string username, string password);

        public Task<IEnumerable<string>> GetUserRole(User user);
    }
}
