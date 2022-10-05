using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Interfaces
{
    public interface IPersonService
    {
        Task<AddPersonResponse> AddUser(AddPersonRequest user);

        Task<Person?> DeleteUser(int userId);

        Task<IEnumerable<Person>> GetAllUsers();

        Task<Person?> GetByID(int id);

        Task<AddPersonResponse> UpdateUser(AddPersonRequest user, int id);
    }
}
