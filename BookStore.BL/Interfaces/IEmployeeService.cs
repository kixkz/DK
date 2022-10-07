using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Models.Users;

namespace BookStore.BL.Interfaces
{
    public interface IEmployeeService
    {
        public Task<Employee> AddEmployee(Employee employee);

        public Task<Employee?> DeleteEmployee(int employeeId);

        public Task<IEnumerable<Employee>> GetAllEmployees();

        public Task<Employee?> GetByID(int id);

        public Task<Employee> UpdateEmployee(Employee employee);

        public Task<Employee?> GetEmployeeByName(string employee);

        public Task<bool> AddMultipleEmployees(IEnumerable<Employee> employeeCollection);

        public Task<User?> GetUserInfoAsync(string email, string password);
    }
}
