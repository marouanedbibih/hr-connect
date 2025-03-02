using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Departement;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/departement")]
    [ApiController]
    public class DepartementController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }

        public DepartementController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD 

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartementCreateDto dto)
        {
            try
            {
                Departement newDepartment = new Departement
                {
                    Name = dto.Name,
                    Description = dto.Description,
                };

                await _context.Departements.AddAsync(newDepartment);
                await _context.SaveChangesAsync();

                return Ok("Department Created Successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // Read
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departement>>> GetDepartements(int page = 1, int pageSize = 5)
        {
            try
            {
                var totalDepartements = await _context.Departements.CountAsync();

                var departements = await _context.Departements
                    .OrderByDescending(d => d.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalDepartements / pageSize);

                return Ok(new
                {
                    departements,
                    currentPage = page,
                    totalPages
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }


        // Read (Get Department By ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Departement>> GetDepartmentById(long id)
        {
            try
            {
                var department = await _context.Departements.FindAsync(id);

                if (department == null)
                {
                    return NotFound("Department not found");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }


        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(long id, [FromBody] DepartementUpdateDto dto)
        {
            try
            {
                var existingDepartment = await _context.Departements.FindAsync(id);

                if (existingDepartment == null)
                {
                    return NotFound("Department not found");
                }

                // Update the properties based on the DTO
                existingDepartment.Name = dto.Name;
                existingDepartment.Description = dto.Description;

                _context.Departements.Update(existingDepartment);
                await _context.SaveChangesAsync();

                return Ok("Department updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            try
            {
                var departmentToDelete = await _context.Departements.FindAsync(id);

                if (departmentToDelete == null)
                {
                    return NotFound("Department not found");
                }

                _context.Departements.Remove(departmentToDelete);
                await _context.SaveChangesAsync();

                return Ok("Department deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

    }
}
