using backend.Core.Errors;
using backend.Core.Modules.User.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Core.Modules.User
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                return Ok(user);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUserRequest);
                return CreatedAtAction(nameof(GetUserById), user);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(updateUserRequest, id);
                return Ok(updatedUser);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                return StatusCode(ex.Status, ex.Message);
            }
        }
    }
}
