using System.Net;
using AutoMapper;
using BookStore.BL.Interfaces;
using BookStore.BL.Kafka;
using BookStore.BL.Services;
using BookStore.Models.Configurations;
using BookStore.Models.Models;
using BookStore.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly Producer<int, Person> _producer;
        private readonly IList<Person> _persons = KafkaDataList<Person>.persons;
        private readonly IOptionsMonitor<MyKafkaProducerSettings> _optionsMonitor;


        public UserController(ILogger<UserController> logger, IPersonService personRepository, IMapper mapper, Producer<int, Person> producer, IOptionsMonitor<MyKafkaProducerSettings> optionsMonitor)
        {
            _logger = logger;
            _personService = personRepository;
            _mapper = mapper;
            _producer = producer;
            _optionsMonitor = optionsMonitor;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Get all persons")]
        public async Task<IEnumerable<Person>> Get()
        {
            return await _personService.GetAllUsers();
        }

        //[HttpGet(nameof(GetById))]
        //public async Task<Person> GetById(int id)
        //{
        //    if (id <= 0) return BadRequest($"Parameter id: {id} must be greater than zero !");
        //    var result = await _personService


        //    //return _personService.GetByID(id);
        //}

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(Add))]
        public async Task<IActionResult> Add([FromBody] AddPersonRequest personRequest)
        {
            var result = await _personService.AddUser(personRequest);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> Update(AddPersonRequest user, int id)
        {
            return Ok(await _personService.UpdateUser(user, id));
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _personService.DeleteUser(id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Produce")]
        public async Task<IActionResult> ProducerPersons(int key, Person person)
        {
            await _producer.SendMessage(key, person);

            return Ok();
            
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Consume")]
        public async Task<IEnumerable<Person>> GetPersons()
        {
            return _persons;
        }
    }
}