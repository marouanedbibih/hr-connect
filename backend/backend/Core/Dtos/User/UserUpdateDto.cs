namespace backend.Core.Dtos.User
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Password { get; set; }
        public string? Confirm { get; set; }
    }
}
