using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person> AddUser(Person user);

        public Task<Person?> DeleteUser(int userId);

        public Task<IEnumerable<Person>> GetAllUsers();

        public Task<Person?> GetByID(int id);

        public Task<Person> UpdateUser(Person user);

    }
}