namespace backend.Core.Dtos.CompanyDepartement
{
    public class CompanyWithDepartmentsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }

        public List<DepartementDto> Departments { get; set; }

        public int CurrentPage {  get; set; }
        public int TotalPages {  get; set; }

    }
}
