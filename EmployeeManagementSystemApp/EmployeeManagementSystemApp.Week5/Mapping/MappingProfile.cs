using AutoMapper;
using EmployeeManagementSystemApp.Week5.Dtos;
using EmployeeManagementSystemApp.Week5.Models;

namespace EmployeeManagementSystemApp.Week5.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Skill, SkillResponse>();

            CreateMap<Department, DepartmentSummaryResponse>();

            CreateMap<Employee, EmployeeSummaryResponse>()
                .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.FirstName + " " + source.LastName));

            CreateMap<Employee, EmployeeResponse>()
                .ForMember(destination => destination.Department, options => options.MapFrom(source => source.Department))
                .ForMember(destination => destination.Skills, options => options.MapFrom(source => source.Skills));

            CreateMap<Department, DepartmentDetailResponse>()
                .ForMember(destination => destination.Employees, options => options.MapFrom(source => source.Employees));
        }
    }
}
