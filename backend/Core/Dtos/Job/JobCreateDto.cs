using backend.Core.Enums;

namespace backend.Core.Dtos.Job
{
    public class JobCreateDto
    {
        public string Name { get; set; }
        /*public JobLevel Level { get; set; }*/
        public string Description { get; set; }
    }
}
