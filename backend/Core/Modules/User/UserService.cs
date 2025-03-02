using backend.Core.Repositories;
using backend.Core.Errors;
using backend.Core.Modules.User.Http;
using AutoMapper;

namespace backend.Core.Modules.User
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("id", $"User with ID {id} not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new BusinessException($"User with username {username} not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new BusinessException($"User with email {email} not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return _mapper.Map<IEnumerable<UserDTO>>(await _userRepository.GetUsersAsync());
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(createUserRequest.Username);
            if (existingUserByUsername != null)
                throw new BusinessException($"Username {createUserRequest.Username} already taken.", StatusCodes.Status409Conflict);

            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(createUserRequest.Email);
            if (existingUserByEmail != null)
                throw new BusinessException($"Email {createUserRequest.Email} already taken.", StatusCodes.Status409Conflict);

            var user = _mapper.Map<User>(createUserRequest);

            return _mapper.Map<UserDTO>(await _userRepository.CreateUserAsync(user));
        }

        public async Task<UserDTO> UpdateUserAsync(UpdateUserRequest request, long ID)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(ID);
            if (existingUser == null)
                throw new BusinessException("id", $"User with ID {ID} not found.", StatusCodes.Status404NotFound);

            var user = _mapper.Map(request, existingUser);

            return _mapper.Map<UserDTO>(await _userRepository.UpdateUserAsync(user));
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new BusinessException("id", $"User with ID {id} not found.", StatusCodes.Status404NotFound);
        }
    }
}
