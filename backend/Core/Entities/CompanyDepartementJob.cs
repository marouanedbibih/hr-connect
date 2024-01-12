namespace backend.Core.Entities
{
    public class CompanyDepartementJob
    {
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        public long DepartementId { get; set; }
        public Departement Departement { get; set; }

        public long JobId { get; set; }
        public Job Job { get; set; }
    }
}
