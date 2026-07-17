using Application.DTOs;
using AutoMapper;
using Domain.Entity;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(x => x.Department.Name));
            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Department , DepartmentDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
        }
    }
}
