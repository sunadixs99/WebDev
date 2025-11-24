using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using Demo.API.Models;
using Demo.API.Dto;
namespace Demo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    public EmployeesController()
    {

    }

    [HttpGet]

    public async Task<IActionResult> GetEmployees()
    {
        using (IDbConnection dbConnection = CreateConnection())
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Open();

            string sql = "SELECT * FROM employee";

            var employees = await dbConnection.QueryAsync<Employee>(sql);

            return Ok(employees);
        }
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetEmployee(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            using (IDbConnection dbConnection = CreateConnection())
            {
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Open();

                string sql = "SELECT * FROM employee WHERE id = @id";

                var employees = await dbConnection.QueryFirstOrDefaultAsync<Employee>(sql, new { id });

                return Ok(employees);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost]

    public async Task<IActionResult> CreateEmployee(CreateEmployeeDto employee)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(employee);
            ArgumentException.ThrowIfNullOrEmpty(employee.fullname);
            ArgumentException.ThrowIfNullOrEmpty(employee.address);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(employee.age);

            using (IDbConnection dbConnection = CreateConnection())
            {
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Open();

                string sql = "insert into employee (fullname, address, age, birthday) values (@fullname, @address, @age, @birthday);";

                try
                {
                    await dbConnection.ExecuteAsync(sql, employee);

                    return Ok("Employee created successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = "Error creating employee", Details = ex.Message });
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Invalid employee data", Details = ex.Message });
        }

    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteEmployee(int id)
    {
        using (IDbConnection dbConnection = CreateConnection())
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Open();

            string sql = "DELETE FROM employee WHERE id = @id";

            var affectedRows = await dbConnection.ExecuteAsync(sql, new { id });

            if (affectedRows > 0)
            {
                return Ok("Employee deleted successfully");
            }
            else
            {
                return Ok("No employee found with the given id");
            }
        }
    }

    private IDbConnection CreateConnection()
    {
        string connectionString = "Server=localhost;Port=3306;Database=dapperdemodb;User Id=root;Password=;";
        return new MySqlConnection(connectionString);
    }
}
