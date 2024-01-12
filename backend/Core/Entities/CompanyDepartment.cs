namespace backend.Core.Entities
{
    public class CompanyDepartment
    {
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        public long DepartmentId { get; set; }
        public Departement Departement { get; set; }
    }
}
