// using AutoMapper;
// using backend.Core.Context;
// using backend.Core.Dtos.Admin;
// using backend.Core.Dtos.User;
// using backend.Core.Entities;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using System.Collections.Generic;
// using System.Data;
// using System.Threading.Tasks;
// using static System.Runtime.InteropServices.JavaScript.JSType;
// using Microsoft.Extensions.Logging;

// namespace backend.Controllers
// {
//     [Route("api/admin")]
//     [ApiController]
//     public class AdminController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IMapper _mapper;
//         private readonly ILogger<AdminController> _logger;

//         public AdminController(ApplicationDbContext context, IMapper mapper, ILogger<AdminController> logger)
//         {
//             _context = context;
//             _mapper = mapper;
//             _logger = logger;
//         }

//         [HttpPost]
//        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
//         {
//             var errors = new List<string>();
//             try
//             {
//                 if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Phone) ||
//                     string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Password) ||
//                     string.IsNullOrEmpty(dto.Confirm))
//                 {
//                     errors.Add("All input fields are required");
//                     return BadRequest(new { errors });
//                 }

//                 if (dto.Password != dto.Confirm)
//                 {
//                     errors.Add("Confirm password does not match");
//                     return BadRequest(new { errors });
//                 }

//                 // Map UserCreateDto to User using AutoMapper
//                 User newUser = new User
//                 {
//                     Name = dto.Name,
//                     Email = dto.Email,
//                     Phone = dto.Phone,
//                     Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
//                     Role = "admin",
//                 };

//                 // Save the user to the Users table
//                 await _context.Users.AddAsync(newUser);
//                 await _context.SaveChangesAsync();

//                 // Get the user ID
//                 long userId = newUser.ID;

//                 // Insert only the UserID into the Admins table
//                 var newAdmin = new Admin
//                 {
//                     UserId = userId
//                 };

//                 await _context.Admins.AddAsync(newAdmin);
//                 await _context.SaveChangesAsync();

//                 return Ok(new { userId, message = "Admin Created Successfully" });
//             }
//             catch (DbUpdateException ex) when (IsDuplicateEmailError(ex))
//             {
//                 errors.Add("Email already exists");
//                 return BadRequest(new { errors });
//             }
//         }

//         [HttpPut("{adminId}")]
// /*        [Route("Update/{adminId}")]
// */        public async Task<IActionResult> UpdateAdmin(long adminId, [FromBody] UserUpdateDto dto)
//         {
//             var errors = new List<string>();
//             try
//             {
//                 // Validate required fields
//                 if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Phone) || string.IsNullOrEmpty(dto.Name))
//                 {
//                     errors.Add("All input fields are required");
//                     return BadRequest(new { errors });
//                 }

//                 // Find the Admin in the database including the associated User
//                 var admin = await _context.Admins
//                     .Include(a => a.User) // Include the User navigation property
//                     .FirstOrDefaultAsync(a => a.ID == adminId);

//                 // Check if the Admin with the given ID exists
//                 if (admin == null)
//                 {
//                     return NotFound(new { message = "Admin not found" });
//                 }

//                 // Access the associated User
//                 var existingUser = admin.User;

//                 // Update user properties
//                 _mapper.Map(dto, existingUser);

//                 // If password fields are provided, update password
//                 if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.Confirm))
//                 {
//                     if (dto.Password != dto.Confirm)
//                     {
//                         errors.Add("Confirm password does not match");
//                         return BadRequest(new { errors });
//                     }

//                     // Update password if both Password and Confirm are provided and match
//                     existingUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
//                 }

//                 // Save changes to the User entity
//                 await _context.SaveChangesAsync();

//                 return Ok(new { message = "Admin and associated User updated successfully" });
//             }
//             catch (DbUpdateException ex) when (IsDuplicateEmailError(ex))
//             {
//                 return BadRequest(new { message = "Email already exists" });
//             }
//         }


//         [HttpGet("{adminId}")]
// /*        [Route("Get/{adminId}")]
// */        public async Task<IActionResult> GetAdminById(long adminId)
//         {
//             try
//             {
//                 // Find the Admin in the database including the associated User
//                 var admin = await _context.Admins
//                     .Include(a => a.User) // Include the User navigation property
//                     .FirstOrDefaultAsync(a => a.ID == adminId);

//                 // Check if the Admin with the given ID exists
//                 if (admin == null)
//                 {
//                     return NotFound(new { message = "Admin not found" });
//                 }

//                 // Access the associated User
//                 var user = admin.User;

//                 // Return both Admin and User information
//                 return Ok(new { adminId = admin.ID, user });
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error" });
//             }
//         }


//         [HttpDelete("{adminId}")]
// /*        [Route("Delete/{adminId}")]
// */        public async Task<IActionResult> DeleteAdminAndUser(long adminId)
//         {
//             try
//             {
//                 // Find the Admin in the database including the associated User
//                 var admin = await _context.Admins
//                     .Include(a => a.User) // Include the User navigation property
//                     .FirstOrDefaultAsync(a => a.ID == adminId);

//                 // Check if the Admin with the given ID exists
//                 if (admin == null)
//                 {
//                     return NotFound(new { message = "Admin not found" });
//                 }

//                 // Access the associated User
//                 var user = admin.User;

//                 // Check if the User with the associated ID exists
//                 if (user == null)
//                 {
//                     return NotFound(new { message = "User not found" });
//                 }

//                 // Remove the User entity from the context
//                 _context.Users.Remove(user);

//                 // Remove the Admin entity from the context
//                 _context.Admins.Remove(admin);

//                 // Save changes to the database
//                 await _context.SaveChangesAsync();

//                 return Ok(new { message = "Admin  deleted successfully" });
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
//             }
//         }

//         [HttpGet]

//         public async Task<IActionResult> GetAllAdmins(int page = 1, int pageSize = 5)
//         {
//             try
//             {
//                 var totalAdmins = await _context.Admins.CountAsync();

//                 var admins = await _context.Admins
//                     .Include(a => a.User)
//                     .OrderByDescending(a => a.CreatedAt) // Assuming CreatedDate is the property representing the date created
//                     .Skip((page - 1) * pageSize)
//                     .Take(pageSize)
//                     .Select(a => new
//                     {
//                         adminId = a.ID,
//                         user = a.User
//                     })
//                     .ToListAsync();

//                 var totalPages = (int)Math.Ceiling((double)totalAdmins / pageSize);

//                 return Ok(new
//                 {
//                     admins,
//                     currentPage = page,
//                     totalPages
//                 });
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception or handle it as needed
//                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", errorDetails = ex.Message, stackTrace = ex.StackTrace });
//             }
//         }


















//         private bool IsDuplicateEmailError(DbUpdateException ex)
//         {
//             const int SqlServerErrorNumberForDuplicateKey = 2601;

//             return ex.InnerException is SqlException sqlException &&
//                    sqlException.Number == SqlServerErrorNumberForDuplicateKey;
//         }
//     }
// }
