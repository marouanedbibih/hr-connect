// using AutoMapper;
// using backend.Core.Context;
// using backend.Core.Dtos.CompanyDepartement;
// using backend.Core.Entities;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace backend.Controllers
// {
//     [Route("api/company-departes")]
//     [ApiController]
//     public class CompanyDepartementController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IMapper _mapper;
//         private readonly ILogger<AdminController> _logger;

//         public CompanyDepartementController(ApplicationDbContext context, IMapper mapper, ILogger<AdminController> logger)
//         {
//             _context = context;
//             _mapper = mapper;
//             _logger = logger;
//         }

//         [HttpGet("company/{companyId}/departments")]
//         public async Task<ActionResult<CompanyWithDepartmentsDto>> GetCompanyDepartments(long companyId, int page = 1, int pageSize = 5)
//         {
//             try
//             {
//                 var company = await _context.Companies
//                     .Include(c => c.CompanyDepartments)
//                     .ThenInclude(cd => cd.Departement)
//                     .FirstOrDefaultAsync(c => c.ID == companyId);

//                 if (company == null)
//                 {
//                     return NotFound("Company not found");
//                 }

//                 var totalDepartments = company.CompanyDepartments.Count;

//                 // Calculate the total number of pages
//                 var totalPages = (int)Math.Ceiling((double)totalDepartments / pageSize);

//                 var departments = company.CompanyDepartments
//                     .OrderBy(cd => cd.Departement.CreatedAt) // Adjust the ordering based on your needs
//                     .Skip((page - 1) * pageSize)
//                     .Take(pageSize)
//                     .Select(cd => new DepartementDto
//                     {
//                         Id = cd.Departement.ID,
//                         Name = cd.Departement.Name,
//                         Description = cd.Departement.Description,
//                     })
//                     .ToList();

//                 var result = new CompanyWithDepartmentsDto
//                 {
//                     Id = company.ID,
//                     Name = company.Name,
//                     Size = company.Size.ToString(),
//                     Departments = departments,
//                     CurrentPage = page,
//                     TotalPages = totalPages
//                 };

//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpGet("company/{companyId}/departments/not-in-company")]
//         public async Task<ActionResult<IEnumerable<DepartementDto>>> DepartesNotInCompany(long companyId)
//         {
//             try
//             {
//                 var company = await _context.Companies
//                     .Include(c => c.CompanyDepartments)
//                     .ThenInclude(cd => cd.Departement)
//                     .FirstOrDefaultAsync(c => c.ID == companyId);

//                 if (company == null)
//                 {
//                     return NotFound("Company not found");
//                 }

//                 // Get all departments that are not in the company
//                 var allDepartments = await _context.Departements.ToListAsync();
//                 var departmentsNotInCompany = allDepartments
//                     .Where(d => !company.CompanyDepartments.Any(cd => cd.Departement.ID == d.ID))
//                     .Select(d => new DepartementDto
//                     {
//                         Id = d.ID,
//                         Name = d.Name,
//                         Description = d.Description,
//                     })
//                     .ToList();

//                 return Ok(departmentsNotInCompany);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpPost("add-department-to-company")]
//         public async Task<IActionResult> AddDepartementToCompany([FromBody] CompanyDepartementDto dto)
//         {
//             try
//             {
//                 // Check if the company and department exist
//                 var company = await _context.Companies.FindAsync(dto.CompanyId);
//                 var department = await _context.Departements.FindAsync(dto.DepartementId);

//                 if (company == null || department == null)
//                 {
//                     return NotFound("Company or department not found");
//                 }

//                 // Check if the association already exists
//                 if (_context.CompanyDepartments.Any(cd => cd.CompanyId == dto.CompanyId && cd.DepartmentId == dto.DepartementId))
//                 {
//                     return BadRequest("Department is already associated with the company");
//                 }

//                 // Create a new CompanyDepartment entity
//                 var companyDepartmentEntity = new CompanyDepartment
//                 {
//                     CompanyId = dto.CompanyId,
//                     DepartmentId = dto.DepartementId
//                     // You can set other properties if needed
//                 };

//                 // Add to the context and save changes
//                 _context.CompanyDepartments.Add(companyDepartmentEntity);
//                 await _context.SaveChangesAsync();

//                 // Optionally, you can return a list of updated DepartementDto objects
//                 var updatedDepartements = await GetCompanyDepartments(dto.CompanyId);
//                 return Ok(updatedDepartements);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpGet("company/{companyId}/all-departes")]
//         public async Task<ActionResult<CompanyWithDepartmentsDto>> GetAllDepartesOfCompany(long companyId)
//         {
//             try
//             {
//                 var company = await _context.Companies
//                     .Include(c => c.CompanyDepartments)
//                     .ThenInclude(cd => cd.Departement)
//                     .FirstOrDefaultAsync(c => c.ID == companyId);

//                 if (company == null)
//                 {
//                     return NotFound("Company not found");
//                 }


//                 var departments = company.CompanyDepartments
//                     .OrderBy(cd => cd.Departement.CreatedAt) // Adjust the ordering based on your needs
//                     .Select(cd => new
//                     {
//                         Id = cd.Departement.ID,
//                         Name = cd.Departement.Name,
//                     })
//                     .ToList();

//                 var result = new
//                 {
//                     Departments = departments,
//                 };

//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpDelete("{companyId}/{departementId}")]
//         public async Task<IActionResult> RemoveDepartmentFromCompany([FromRoute] long companyId, [FromRoute] long departementId)
//         {
//             try
//             {
//                 // Check if the company and department exist
//                 var company = await _context.Companies.FindAsync(companyId);
//                 var department = await _context.Departements.FindAsync(departementId);

//                 if (company == null || department == null)
//                 {
//                     return NotFound("Company or department not found");
//                 }

//                 // Check if the association exists
//                 var companyDepartmentEntity = await _context.CompanyDepartments
//                     .FirstOrDefaultAsync(cd => cd.CompanyId == companyId && cd.DepartmentId == departementId);

//                 if (companyDepartmentEntity == null)
//                 {
//                     return BadRequest("Department is not associated with the company");
//                 }

//                 // Remove the association from the context and save changes
//                 _context.CompanyDepartments.Remove(companyDepartmentEntity);
//                 await _context.SaveChangesAsync();

//                 return Ok(new { message = "Your Company was deleted successfully" });
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }


//     }
// }
