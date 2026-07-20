using Application.DTOs;
using AutoMapper;
using Domain.Entity;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Employee, EmployeeDTO>()
            //    .ForMember(d => d.DepartmentName, opt => opt.MapFrom(x => x.Department.Name));
            CreateMap<EmployeeDTO, Employee>()
       .ForMember(x => x.Department, opt => opt.Ignore())
       .ForMember(x => x.DepartmentId, opt => opt.MapFrom(x => x.DepartmentId));
            CreateMap<Employee , EmployeeDTO>();

            CreateMap<Department , DepartmentDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
    
        }
    }
}
