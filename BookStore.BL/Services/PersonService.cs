using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;

namespace BookStore.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Person AddUser(Person user)
        {
            return  _personRepository.AddUser(user);
        }

        public Person? DeleteUser(int userId)
        {
            return _personRepository.DeleteUser(userId);
        }

        public IEnumerable<Person> GetAllUsers()
        {
            return _personRepository.GetAllUsers();
        }

        public Person? GetByID(int id)
        {
            return _personRepository.GetByID(id);
        }

        public Person UpdateUser(Person user)
        {
            return _personRepository.UpdateUser(user);
        }
    }
}