using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.DL.Repositories.InMemotyRepositories
{
    public class UserInMemoryRepository
    {
        private static List<Person> _users = new List<Person>()
        {
            new Person()
            {
                Id = 1,
                Name = "Toshko",
                Age = 35
            },

            new Person()
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

        public IEnumerable<Person> GetAllUsers()
        {
            return _users;
        }

        public Person? GetByID(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public Person AddUser(Person user)
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

        public Person UpdateUser(Person user)
        {
            var existingUser = _users.FirstOrDefault(x => x.Id == user.Id);
            if (existingUser == null) return null;

            _users.Remove(existingUser);

            _users.Add(user);

            return user;
        }

        public Person? DeleteUser(int userId)
        {
            if (userId < 0) return null;

            var user = _users.FirstOrDefault(x => x.Id == userId);

            _users.Remove(user);

            if (user != null) return user;

            return user;
        }


    }
}