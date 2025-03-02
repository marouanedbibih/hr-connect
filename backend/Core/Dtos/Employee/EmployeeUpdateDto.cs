using backend.Core.Enums;

namespace backend.Core.Dtos.Employee
{
    public class EmployeeUpdateDto
    {
        // User Dto
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Password { get; set; }
        public string? Confirm { get; set; }

        // Employe Dto
        public long CompanyId { get; set; }
        public long DepartementId { get; set; }
        public long JobId { get; set; }
        public JobLevel Level { get; set; }
    }
}
