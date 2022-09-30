using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.DL.Repositories
{
    public class UserInMemoryRepository : IPersonRepository
    {
        private static List<User> _users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Name = "Toshko",
                Age = 35
            },

            new User()
            {
                Id = 2,
                Name = "Peshko",
                Age = 30
            },
        };

        public Guid Id { get; set; }

        public UserInMemoryRepository()
        {
            
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User? GetByID(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User AddUser(User user)
        {
            try
            {
                _users.Add(user);
            }
            catch (Exception e)
            {
                return null;
            }

            return user;
        }

        public User UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(x => x.Id == user.Id);
            if (existingUser == null) return null;

            _users.Remove(existingUser);

            _users.Add(user);

            return user;
        }

        public User? DeleteUser(int userId)
        {
            if (userId < 0) return null;

            var user = _users.FirstOrDefault(x => x.Id == userId);

            _users.Remove(user);

            if (user != null) return user;

            return user;
        }

        
    }
}