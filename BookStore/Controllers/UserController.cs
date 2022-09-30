using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPersonRepository _userInMemoryRepository;
        private static readonly List<User> _models = new List<User>();


        private readonly ILogger<User> _logger;

        public UserController(ILogger<User> logger, IPersonRepository userInMemoryRepository)
        {
            _logger = logger;
            _userInMemoryRepository = userInMemoryRepository;
        }

        [HttpGet(nameof(Get))]
        public IEnumerable<User> Get()
        {
            return _userInMemoryRepository.GetAllUsers();
        }

        [HttpGet(nameof(GetById))]
        public User GetById(int id)
        {
            return _userInMemoryRepository.GetByID(id);
        }

        [HttpPost]
        public void Add([FromBody] User model)
        {
            _userInMemoryRepository.AddUser(model);
        }

        [HttpPut]
        public void Update(User user)
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