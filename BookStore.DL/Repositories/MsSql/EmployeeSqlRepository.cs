using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DL.Interfaces;
using BookStore.Models.Models;
using BookStore.Models.Models.Users;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStore.DL.Repositories.MsSql
{
    public class EmployeeSqlRepository : IEmployeeRepository
    {
        private readonly ILogger<EmployeeSqlRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeSqlRepository(IConfiguration configuration, ILogger<EmployeeSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Employee] (EmployeeID, NationalIDNumber, EmployeeName, LoginID, JobTitle, BirthDate, MaritalStatus, Gender, HireDate, VacationHours, SickLeaveHours, rowguid, ModifiedDate) VALUES (@EmployeeID, @NationalIDNumber, @EmployeeName, @LoginID, @JobTitle, @BirthDate, @MaritalStatus, @Gender, @HireDate, @VacationHours, @SickLeaveHours, @rowguid, @ModifiedDate)", employee);

                    return employee;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddEmployee)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<bool> AddMultipleEmployees(IEnumerable<Employee> employeeCollection)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [Employee] (EmployeeId, NationalIDNumber, EmployeeName, LoginID, JobTitle, BirthDate, MaritalStatus, Gender, HireDate, VacationHours, SickLeaveHours, rowguid, ModifiedDate) VALUES (@NationalIDNumber, @EmployeeName, @LoginID, @JobTitle, @BirthDate, @MaritalStatus, @Gender, @HireDate, @VacationHours, @SickLeaveHours, @rowguid, @ModifiedDate))", employeeCollection);
                    return result > 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddMultipleEmployees)}:{e.Message}", e);
            }

            return false;
        }

        public async Task<Employee?> DeleteEmployee(int employeeId)
        {
            var deletedEmployee = await GetByID(employeeId);

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();


                    var result = await conn.ExecuteAsync("DELETE FROM Employee WHERE Id=@id", new { id = employeeId });

                    return deletedEmployee;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteEmployee)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Employee WITH(NOLOCK)";

                    await conn.OpenAsync();

                    var result = await conn.QueryAsync<Employee>(query);

                    return result;


                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllEmployees)}:{e.Message}", e);
            }

            return Enumerable.Empty<Employee>();
        }

        public async Task<Employee?> GetByID(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employee WITH(NOLOCK) WHERE EmployeeID = @EmployeeID", new { EmployeeID = id });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetByID)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Employee?> GetEmployeeByName(string employeeName)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employee WHERE EmployeeName = @EmployeeName WITH(NOLOCK)", new { EmployeeName = employeeName });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetEmployeeByName)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<User> GetUserInfoAsync(string email, string password)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM UserInfo WHERE Email=@email AND Password=@password", new { email = email, password = password });
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetUserInfoAsync)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    await conn.QueryFirstOrDefaultAsync<Employee>("UPDATE Employee SET NationalIDNumber=@nationalIDNumber, EmployeeName=@employeeName, LoginID=@loginID, JobTitle=@jobTitle, BirthDate=@birthDate, MaritalStatus=@maritalStatus, Gender=@gender, HireDate=@hireDate, VacationHours=@vacationHours, SickLeaveHours=@sickLeaveHours, rowguid=@rowguid, ModifiedDate=@modifiedDate WHERE EmployeeID=@employeeID",
                               new { employeeID = employee.EmployeeId, nationalIDNumber = employee.NationalIDNumber, employeeName = employee.EmployeeName, loginID = employee.LoginId, jobTitle = employee.JobTitle, birthDate = employee.BirthDate, maritalStatus = employee.MaritalStatus, gender = employee.Gender, hireDate = employee.HireDate, vacationHours = employee.VacationHours, sickLeaveHours = employee.SickLeaveHours, rowguid = employee.Rowguid, modifiedDate = employee.ModifiedDate});
                    return employee;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateEmployee)}:{e.Message}", e);
            }

            return null;
        }
    }
}
