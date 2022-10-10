using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.BL.Interfaces;
using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RoleManager<UserRole> _roleManager;

        public IdentityService(UserManager<User> userManager, IPasswordHasher<User> passwordHasher, RoleManager<UserRole> roleManager)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
        }

        public async Task<User> CheckUserAndPass(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<IdentityResult> CreateAsync(User user)
        {
            var existingUser = await _userManager.GetUserIdAsync(user);

            if (string.IsNullOrEmpty(existingUser))
            {
                return await _userManager.CreateAsync(user);
            }

            return IdentityResult.Failed();
        }

        public async Task<IEnumerable<string>> GetUserRole(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
