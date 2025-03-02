using backend.Core.Modules.User;

namespace backend.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(long id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}