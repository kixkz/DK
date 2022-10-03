using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPersonService _userInMemoryRepository;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IPersonService userInMemoryRepository)
        {
            _logger = logger;
            _userInMemoryRepository = userInMemoryRepository;
        }

        [HttpGet(nameof(Get))]
        public IEnumerable<Person> Get()
        {
            return _userInMemoryRepository.GetAllUsers();
        }

        [HttpGet(nameof(GetById))]
        public Person GetById(int id)
        {
            return _userInMemoryRepository.GetByID(id);
        }

        [HttpPost]
        public void Add([FromBody] Person model)
        {
            _userInMemoryRepository.AddUser(model);
        }

        [HttpPut]
        public void Update(Person user)
        {
            _userInMemoryRepository.UpdateUser(user);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _userInMemoryRepository.DeleteUser(id);
        }

       
    }
}