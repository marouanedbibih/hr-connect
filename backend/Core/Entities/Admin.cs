using System.ComponentModel.DataAnnotations;
namespace backend.Core.Entities
{

    public class Admin : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }
    }
}
