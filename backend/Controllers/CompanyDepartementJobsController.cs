// using AutoMapper;
// using backend.Core.Context;
// using backend.Core.Entities;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using backend.Core.Dtos.CompanyDepartementJobs;

// namespace backend.Controllers
// {
//     [Route("api/company-departement-job")]
//     [ApiController]
//     public class CompanyDepartementJobsController : ControllerBase
//     {
//         private ApplicationDbContext _context { get; }
//         private IMapper _mapper { get; }

//         public CompanyDepartementJobsController(ApplicationDbContext context, IMapper mapper)
//         {
//             _context = context;
//             _mapper = mapper;
//         }
//         [HttpPost]
//         public async Task<bool> AddJobToDepartementCompany(CompanyDepartementJobDto dto)
//         {
//             // Check if Company exists
//             var company = await _context.Companies.FindAsync(dto.CompanyId);
//             if (company == null)
//             {
//                 return false;
//             }

//             // Check if Departement exists
//             var departement = await _context.Departements.FindAsync(dto.DepartementId);
//             if (departement == null)
//             {
//                 return false;
//             }

//             // Check if Job exists
//             var job = await _context.Jobs.FindAsync(dto.JobId);
//             if (job == null)
//             {
//                 return false;
//             }

//             // Check if the combination already exists
//             if (_context.CompanyDepartementJobs.Any(cdj =>
//                 cdj.CompanyId == dto.CompanyId &&
//                 cdj.DepartementId == dto.DepartementId &&
//                 cdj.JobId == dto.JobId))
//             {
//                 return false; // Combination already exists
//             }

//             // If all checks pass, create and add the CompanyDepartementJob entity
//             var newCompanyDepartementJob = new CompanyDepartementJob
//             {
//                 CompanyId = dto.CompanyId,
//                 DepartementId = dto.DepartementId,
//                 JobId = dto.JobId,
//                 // Set other properties if needed...
//             };

//             _context.CompanyDepartementJobs.Add(newCompanyDepartementJob);
//             await _context.SaveChangesAsync();

//             return true; // Successfully added
//         }


//         [HttpGet("{companyId}/{departmentId}")]
//         public async Task<ActionResult<IEnumerable<object>>> GetJobs(long departmentId, long companyId, int page = 1, int pageSize = 5)
//         {
//             try
//             {
//                 var company = await _context.Companies.FindAsync(companyId);
//                 var department = await _context.Departements.FindAsync(departmentId);

//                 if (company == null || department == null)
//                 {
//                     return NotFound("Company or department not found");
//                 }

//                 var totalJobs = await _context.CompanyDepartementJobs
//                     .Where(cdj => cdj.DepartementId == departmentId && cdj.CompanyId == companyId)
//                     .Select(cdj => cdj.Job)
//                     .CountAsync();

//                 var totalPages = (int)Math.Ceiling((double)totalJobs / pageSize);

//                 var jobs = await _context.CompanyDepartementJobs
//                     .Where(cdj => cdj.DepartementId == departmentId && cdj.CompanyId == companyId)
//                     .Select(cdj => new
//                     {
//                         Id = cdj.Job.ID,
//                         Name = cdj.Job.Name
//                     })
//                     .OrderBy(j => j.Id)
//                     .Skip((page - 1) * pageSize)
//                     .Take(pageSize)
//                     .ToListAsync();

//                 return Ok(new
//                 {
//                     CompanyName = company.Name,
//                     DepartmentName = department.Name,
//                     Jobs = jobs,
//                     CurrentPage = page,
//                     TotalPages = totalPages
//                 });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpGet("jobs-not-in-company-department/{companyId}/{departmentId}")]
//         public async Task<ActionResult<IEnumerable<object>>> GetJobsNotInCompanyDepartment(long companyId, long departmentId)
//         {
//             try
//             {
//                 // Get all job IDs associated with the specified company and department
//                 var jobIdsInCompanyDepartment = await _context.CompanyDepartementJobs
//                     .Where(cdj => cdj.CompanyId == companyId && cdj.DepartementId == departmentId)
//                     .Select(cdj => cdj.JobId)
//                     .ToListAsync();

//                 // Get only the ID and Name properties of jobs that are not in the specified company and department
//                 var jobsNotInCompanyDepartment = await _context.Jobs
//                     .Where(job => !jobIdsInCompanyDepartment.Contains(job.ID))
//                     .Select(job => new
//                     {
//                         Id = job.ID,
//                         Name = job.Name
//                     })
//                     .ToListAsync();

//                 return Ok(jobsNotInCompanyDepartment);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError, new
//                 {
//                     message = "Internal Server Error",
//                     errorDetails = ex.Message,
//                     stackTrace = ex.StackTrace
//                 });
//             }
//         }

//         [HttpDelete("{companyId}/{departmentId}/jobs/{jobId}")]
//         public async Task<IActionResult> RemoveJobFromDepartment(long companyId, long departmentId, long jobId)
//         {
//             try
//             {
//                 // Check if the relationship exists
//                 var companyDepartementJob = await _context.CompanyDepartementJobs
//                     .FirstOrDefaultAsync(cdj => cdj.CompanyId == companyId && cdj.DepartementId == departmentId && cdj.JobId == jobId);

//                 if (companyDepartementJob == null)
//                 {
//                     return NotFound("CompanyDepartementJob not found");
//                 }

//                 // Remove the job from the department
//                 _context.CompanyDepartementJobs.Remove(companyDepartementJob);
//                 await _context.SaveChangesAsync();

//                 return NoContent(); // 204 No Content
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
//             }
//         }

//         [HttpGet("all/{companyId}/{departmentId}")]
//         public async Task<ActionResult<IEnumerable<object>>> GetAllJobs(long departmentId, long companyId)
//         {
//             try
//             {
//                 var company = await _context.Companies.FindAsync(companyId);
//                 var department = await _context.Departements.FindAsync(departmentId);

//                 if (company == null || department == null)
//                 {
//                     return NotFound("Company or department not found");
//                 }

                
//                 var jobs = await _context.CompanyDepartementJobs
//                     .Where(cdj => cdj.DepartementId == departmentId && cdj.CompanyId == companyId)
//                     .Select(cdj => new
//                     {
//                         Id = cdj.Job.ID,
//                         Name = cdj.Job.Name
//                     })
//                     .OrderBy(j => j.Id)
//                     .ToListAsync();

//                 return Ok(new
//                 {

//                     Jobs = jobs,

//                 });
//             }
//             catch (Exception ex)
//             {
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
