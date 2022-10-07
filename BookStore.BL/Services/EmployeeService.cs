using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories.MsSql;
using BookStore.Models.Models.Users;

namespace BookStore.BL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            return await _employeeRepository.AddEmployee(employee);
        }

        public Task<bool> AddMultipleEmployees(IEnumerable<Employee> employeeCollection)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee?> DeleteEmployee(int employeeId)
        {
            return await _employeeRepository.DeleteEmployee(employeeId);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetAllEmployees();
        }

        public async Task<Employee?> GetByID(int id)
        {
            return await _employeeRepository.GetByID(id);
        }

        public async Task<Employee?> GetEmployeeByName(string employee)
        {
            return await _employeeRepository.GetEmployeeByName(employee);
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            return await _employeeRepository.UpdateEmployee(employee);
        }

        public async Task<User?> GetUserInfoAsync(string email, string password)
        {
            return await _employeeRepository.GetUserInfoAsync(email, password);
        }
    }
}
