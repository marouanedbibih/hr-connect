using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Employee;
using backend.Core.Dtos.User;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ApplicationDbContext context, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var errors = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Phone) ||
                    string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Password) ||
                    string.IsNullOrEmpty(dto.Confirm))
                {
                    errors.Add("All input fields are required");
                    return BadRequest(new { errors });
                }

                if (dto.Password != dto.Confirm)
                {
                    errors.Add("Confirm password does not match");
                    return BadRequest(new { errors });
                }

                // Check if the specified Company, Department, and Job combination exists
                var companyDepartementJobExists = await _context.CompanyDepartementJobs
                    .AnyAsync(cdj => cdj.CompanyId == dto.CompanyId
                                     && cdj.DepartementId == dto.DepartementId
                                     && cdj.JobId == dto.JobId);

                if (!companyDepartementJobExists)
                {
                    errors.Add("Specified Company, Department, and Job combination does not exist");
                    return BadRequest(new { errors });
                }

                // Map UserCreateDto to User using AutoMapper
                User newUser = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = "employee",
                };

                // Save the user to the Users table
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // Get the user ID
                long userId = newUser.ID;

                // Insert only the UserID into the Admins table
                var newEmployee = new Employee
                {
                    UserId = userId,
                    CompanyId = dto.CompanyId,
                    DepartementId = dto.DepartementId,
                    JobId = dto.JobId,
                    Level = dto.Level
                };

                await _context.Employees.AddAsync(newEmployee);
                await _context.SaveChangesAsync();

                return Ok(new { newEmployee, message = "Employee Created Successfully" });
            }
            catch (DbUpdateException ex) when (IsDuplicateEmailError(ex))
            {
                errors.Add("Email already exists");
                return BadRequest(new { errors });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(long id, [FromBody] EmployeeUpdateDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Phone) || string.IsNullOrEmpty(dto.Name))
            {
                errors.Add("All input fields are required");
                return BadRequest(new { errors });
            }

            if (!string.IsNullOrEmpty(dto.Password) || !string.IsNullOrEmpty(dto.Confirm))
            {
                if (dto.Password != dto.Confirm)
                {
                    errors.Add("Confirm password does not match");
                    return BadRequest(new { errors });
                }

                // Hash the password
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }
            // Check if the specified Company, Department, and Job combination exists
            var companyDepartementJobExists = await _context.CompanyDepartementJobs
                .AnyAsync(cdj => cdj.CompanyId == dto.CompanyId
                                 && cdj.DepartementId == dto.DepartementId
                                 && cdj.JobId == dto.JobId);

            if (!companyDepartementJobExists)
            {
                errors.Add("Specified Company, Department, and Job combination does not exist");
                return BadRequest(new { errors });
            }

            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .Where(e => e.ID == id)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                // Update User information
                employee.User.Name = dto.Name;
                employee.User.Email = dto.Email;
                employee.User.Phone = dto.Phone;

                // Update Employee information
                employee.CompanyId = dto.CompanyId;
                employee.DepartementId = dto.DepartementId;
                employee.JobId = dto.JobId;
                employee.Level = dto.Level;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { message = "Employee updated successfully" });
            }
            catch (DbUpdateException ex) when (IsDuplicateEmailError(ex))
            {
                return BadRequest(new { error = "Email already exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetEmployees(int page = 1, int pageSize = 5)
        {
            try
            {
                var query = _context.Employees
                    .Include(e => e.User)
                    .Include(e => e.Company)
                    .Include(e => e.Departement)
                    .Include(e => e.Job)
                    .Select(e => new
                    {
                        e.ID,
                        e.User.Name,
                        e.User.Email,
                        e.User.Phone,
                        CompanyName = e.Company.Name,
                        DepartementName = e.Departement.Name,
                        JobName = e.Job.Name,
                        e.Level,
                    });

                var totalRecords = await query.CountAsync();

                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                if (page < 1)
                {
                    page = 1;
                }
                else if (page > totalPages)
                {
                    page = totalPages;
                }

                var result = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    Employees = result,
                    CurrentPage = page,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee information.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(long id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .Include(e => e.Company)
                    .Include(e => e.Departement)
                    .Include(e => e.Job)
                    .Where(e => e.ID == id)
                    .Select(e => new
                    {
                        e.ID,
                        e.User.Name,
                        e.User.Email,
                        e.User.Phone,
                        CompanyName = e.Company.Name,
                        CompanyId = e.Company.ID,
                        DepartementName = e.Departement.Name,
                        DepartementId = e.Departement.ID,
                        JobName = e.Job.Name,
                        JobId = e.Job.ID,
                        e.Level,
                    })
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .Where(e => e.ID == id)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                _context.Users.Remove(employee.User); // Remove associated user
                _context.Employees.Remove(employee); // Remove employee

                await _context.SaveChangesAsync();

                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }




        private bool IsDuplicateEmailError(DbUpdateException ex)
        {
            const int SqlServerErrorNumberForDuplicateKey = 2601;

            return ex.InnerException is SqlException sqlException &&
                   sqlException.Number == SqlServerErrorNumberForDuplicateKey;
        }
    }
}
