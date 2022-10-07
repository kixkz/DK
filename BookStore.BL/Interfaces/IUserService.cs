using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Models.Users;

namespace BookStore.BL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserInfoAsync(string email, string password);
    }
}
