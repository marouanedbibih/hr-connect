using backend.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace backend.Core.Entities
{
    public class Employee : BaseEntity
    {
        /*public string? ResumeUrl { get; set; }*/
        public JobLevel Level { get; set; }

        // Relations

        public long JobId { get; set; }
        public Job Job { get; set; }

        public long CompanyId { get; set; }
        public Company Company { get; set; }

        public long DepartementId { get; set; }
        public Departement Departement { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }

    }
}
