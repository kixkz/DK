using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.MediatR.Commands;
using BookStore.Models.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(AddEmployee))]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            return Ok(await _employeeService.AddEmployee(employee));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(Get))]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeService.GetAllEmployees();

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetByID))]
        public async Task<IActionResult?> GetByID(int id)
        {
            return Ok(await _employeeService.GetByID(id));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(UpdateEmployee))]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            return Ok(await _employeeService.UpdateEmployee(employee));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(nameof(DeleteEmployee))]
        public async Task<IActionResult?> DeleteEmployee(int employeeId)
        {
            return Ok(await _employeeService.DeleteEmployee(employeeId));
        }
    }
}
