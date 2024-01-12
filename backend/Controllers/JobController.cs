using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }

        public JobController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD 

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateDto dto)
        {
            Job newJob = new Job
            {
                Name = dto.Name,
                Description = dto.Description,
            } ;
            await _context.Jobs.AddAsync(newJob);
            await _context.SaveChangesAsync();

            return Ok("Job Created Successfully");
        }

        // Read
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs(int page = 1, int pageSize = 5)
        {
            try
            {
                var totalJobs = await _context.Jobs.CountAsync();

                var totalPages = (int)Math.Ceiling((double)totalJobs / pageSize);

                var jobs = await _context.Jobs
                    .OrderBy(j => j.ID) // Adjust the ordering based on your needs
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    Jobs = jobs,
                    CurrentPage = page,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // Read (Get Job By ID)

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(long id)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id);

                if (job == null)
                {
                    return NotFound("Job not found");
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
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
        public async Task<IActionResult> UpdateJob(long id, [FromBody] JobUpdateDto jobUpdateDto)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id);

                if (job == null)
                {
                    return NotFound("Job not found");
                }

                // Update job properties based on the JobUpdateDto
                job.Name = jobUpdateDto.Name;
                job.Description = jobUpdateDto.Description;

                // Mark the entity as modified and save changes
                _context.Entry(job).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal Server Error",
                    errorDetails = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            try
            {
                var job = await _context.Jobs.FindAsync(id);

                if (job == null)
                {
                    return NotFound("Job not found");
                }

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
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
