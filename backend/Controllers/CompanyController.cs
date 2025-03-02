using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }

        public CompanyController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD 

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto dto)
        {
            Company newCompany = new Company
            {
                Name = dto.Name,
                Size = dto.Size,
            };
            await _context.Companies.AddAsync(newCompany);
            await _context.SaveChangesAsync();

            return Ok("Companty Created Successfully");
        }

        // Read
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies(int page = 1, int pageSize = 5)
        {
            try
            {
                var totalCompanies = await _context.Companies.CountAsync();

                var companies = await _context.Companies
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCompanies / pageSize);

                return Ok(new
                {
                    companies,
                    currentPage = page,
                    totalPages
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        // Read (Get Company By ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(long id)
        {
            try
            {
                var company = await _context.Companies.FindAsync(id);

                if (company == null)
                {
                    return NotFound("Company not found");
                }

                return Ok(company);
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
        public async Task<IActionResult> UpdateCompany(long id, [FromBody] CompanyUpdateDto dto)
        {
            try
            {
                var existingCompany = await _context.Companies.FindAsync(id);

                if (existingCompany == null)
                {
                    return NotFound("Company not found");
                }

                // Update the properties based on the DTO
                existingCompany.Name = dto.Name;
                existingCompany.Size = dto.Size;

                _context.Companies.Update(existingCompany);
                await _context.SaveChangesAsync();

                return Ok("Company updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            try
            {
                var companyToDelete = await _context.Companies.FindAsync(id);

                if (companyToDelete == null)
                {
                    return NotFound("Company not found");
                }

                _context.Companies.Remove(companyToDelete);
                await _context.SaveChangesAsync();

                return Ok("Company deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
            }
        }
        // [HttpGet("all")]

        // public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        // {
        //     try
        //     {
        //         var companies = await _context.Companies
        //             .OrderByDescending(c => c.CreatedAt)
        //             .Select(c => new { c.ID, c.Name }) // Select only the required properties
        //             .ToListAsync();

        //         return Ok(new
        //         {
        //             companies,
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception or handle it as needed
        //         return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
        //     }
        // }

    }
}
