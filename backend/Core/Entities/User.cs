using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Core.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
