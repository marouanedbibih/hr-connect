using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.Modules.User
{
    public class UserDTO
    {
        [Required]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Phone]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public UserRole Role { get; set; }

        [JsonPropertyName("last_login")]
        public DateTime? LastLogin { get; set; }

        public UserDTO(string username, string firstName, string lastName, string email, UserRole role, string? phone = null, DateTime? lastLogin = null)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role;
            Phone = phone;
            LastLogin = lastLogin;
        }
    }
}
