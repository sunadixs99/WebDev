using System.Data;
using Dapper;
using Demo.API.Dto;
using Demo.API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Demo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPositionsController : ControllerBase
{
    public JobPositionsController()
    {

    }

    [HttpGet]

    public async Task<IActionResult> GetJobPositions()
    {
        using (IDbConnection dbConnection = CreateConnection())
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Open();

            string sql = "SELECT * FROM job_position";

            var jobPositions = await dbConnection.QueryAsync<JobPosition>(sql);

            return Ok(jobPositions);
        }
    }



    [HttpGet("{id}")]

    public async Task<IActionResult> GetJobPosition(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            using (IDbConnection dbConnection = CreateConnection())
            {
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Open();

                string sql = "SELECT * FROM job_position WHERE id = @id";

                var jobPosition = await dbConnection.QueryFirstOrDefaultAsync<JobPosition>(sql, new { id });

                return Ok(jobPosition);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost]

    public async Task<IActionResult> CreateJobPosition(CreateJobPositionDto jobPosition)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(jobPosition);
            ArgumentException.ThrowIfNullOrEmpty(jobPosition.name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(jobPosition.beginning_salary);

            using (IDbConnection dbConnection = CreateConnection())
            {
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Open();

                string sql = @"
                    INSERT INTO job_position 
                        (name, beginning_salary) 
                    VALUES 
                        (@name, @beginning_salary);";

                try
                {
                    await dbConnection.ExecuteAsync(sql, jobPosition);

                    return Ok("Job position created successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = "Error creating job position", Details = ex.Message });
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Invalid job position data", Details = ex.Message });
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobPosition(int id, UpdateJobPositionDto jobPosition)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
            ArgumentNullException.ThrowIfNull(jobPosition);
            ArgumentException.ThrowIfNullOrEmpty(jobPosition.name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(jobPosition.beginning_salary);

            using (IDbConnection dbConnection = CreateConnection())
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                string sql = @"
                UPDATE job_position
                SET name = @name,
                    beginning_salary = @beginning_salary
                WHERE id = @id;
            ";

                try
                {
                    int rowsAffected = await dbConnection.ExecuteAsync(sql, new
                    {
                        id,
                        jobPosition.name,
                        jobPosition.beginning_salary
                    });

                    if (rowsAffected == 0)
                        return NotFound(new { Message = "Job position not found" });

                    return Ok("Job position updated successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = "Error updating job position", Details = ex.Message });
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Invalid job position data", Details = ex.Message });
        }
    }


    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteJobPosition(int id)
    {
        using (IDbConnection dbConnection = CreateConnection())
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Open();

            string sql = "DELETE FROM job_position WHERE id = @id";

            var affectedRows = await dbConnection.ExecuteAsync(sql, new { id });

            if (affectedRows > 0)
            {
                return Ok("Job position deleted successfully");
            }
            else
            {
                return Ok("No job position found with the given id");
            }
        }
    }

    private IDbConnection CreateConnection()
    {
        string connectionString = "Server=localhost;Port=3306;Database=dapperdemodb;User Id=root;Password=;";
        return new MySqlConnection(connectionString);
    }
}


