using AutoMapper;
using backend.Core.Dtos.Admin;
using backend.Core.Dtos.Candidate;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
using backend.Core.Dtos.User;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile()
        {
            // Company
            CreateMap<CompanyCreateDto, Company>();
            CreateMap<Company, CompanyGetDto>();

            // Job
/*            CreateMap<JobCreateDto, Job>();
            CreateMap<Job, JobGetDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Departement.Name));*/

            // Candidate
            CreateMap<CandidateCreateDto, Employee>();
            CreateMap<Employee, CandidateGetDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Name));

            // User
            // CreateMap<UserCreateDto, User>();
            // CreateMap<UserUpdateDto, User>();
            // CreateMap<User, AdminDto>();


        }
    }
}
