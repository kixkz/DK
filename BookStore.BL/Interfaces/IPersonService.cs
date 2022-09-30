using BookStore.Models.Models;

namespace BookStore.BL.Interfaces
{
    public interface IPersonService
    {
        Person AddUser(Person user);

        Person? DeleteUser(int userId);

        IEnumerable<Person> GetAllUsers();

        Person? GetByID(int id);

        Person UpdateUser(Person user);
    }
}
