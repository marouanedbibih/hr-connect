using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.Modules.User.Http
{
    public record UpdateUserRequest
    {
        [Required]
        [MaxLength(50)]
        [JsonPropertyName("username")]
        public string Username { get; init; }

        [JsonPropertyName("password")]
        public string Password { get; init; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("firstName")]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("lastName")]
        public string LastName { get; init; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [JsonPropertyName("email")]
        public string Email { get; init; }

        [Phone]
        [JsonPropertyName("phone")]
        public string? Phone { get; init; }

        [Required]
        [JsonPropertyName("role")]
        public UserRole Role { get; init; }

        [JsonConstructor]
        public UpdateUserRequest(string username,string password, string firstName, string lastName, string email, UserRole role, string? phone = null)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Role = role;
            Phone = phone;
        }
    }
}
