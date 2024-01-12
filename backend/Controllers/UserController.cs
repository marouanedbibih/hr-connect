using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.User;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }
        private readonly ILogger<AdminController> _logger;


        public UserController(ApplicationDbContext context, IMapper mapper, ILogger<AdminController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                // Find the User in the database
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.ID == id);

                // Check if the User with the given ID exists
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "Error while getting user by ID");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
            }
        }

    }
}
