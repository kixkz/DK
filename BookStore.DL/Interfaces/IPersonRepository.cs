using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IPersonRepository
    {
        User AddUser(User user);

        User? DeleteUser(int userId);

        IEnumerable<User> GetAllUsers();

        User? GetByID(int id);

        User UpdateUser(User user);

    }
}