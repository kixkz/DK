using System.Net;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using BookStore.Models.Responses;

namespace BookStore.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<AddPersonResponse> AddUser(AddPersonRequest personRequest)
        {
            
            var person = _mapper.Map<Person>(personRequest);
            var result = _personRepository.AddUser(person);

            return new AddPersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = await result
            };
        }

        public async Task<Person?> DeleteUser(int userId)
        {
            return await _personRepository.DeleteUser(userId);
        }

        public async Task<IEnumerable<Person>> GetAllUsers()
        {
            return await _personRepository.GetAllUsers();
        }

        public async Task<Person?> GetByID(int id)
        {
            return await _personRepository.GetByID(id);
        }

        public async Task<AddPersonResponse> UpdateUser(AddPersonRequest addPersonRequest, int id)
        {
            var person = _mapper.Map<Person>(addPersonRequest);
            person.Id = id;
            var result = _personRepository.UpdateUser(person);
            return new AddPersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = await result
            };
        }
    }
}