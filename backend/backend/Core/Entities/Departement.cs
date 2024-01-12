using System.Globalization;

namespace backend.Core.Entities
{
    public class Departement : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Relations
        public ICollection<CompanyDepartment> CompanyDepartments { get; set; }

        public ICollection<CompanyDepartementJob> CompanyDepartementJobs { get; set; }

        public ICollection<Employee> Employees { get; set; }



    }
}
