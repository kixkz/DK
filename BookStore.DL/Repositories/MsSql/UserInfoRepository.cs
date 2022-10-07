using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DL.Interfaces;
using BookStore.Models.Models.Users;

namespace BookStore.DL.Repositories.MsSql
{
    public class UserInfoRepository : IUserInfoRepository
    {
        public Task<User?> GetUserInfoAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
