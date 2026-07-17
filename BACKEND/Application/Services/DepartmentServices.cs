using Application.DTOs;
using Application.Interface;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;

namespace Application.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IMapper _mapper;
        private readonly IDepartment _departmentRepository;

        public DepartmentServices(IMapper mapper, IDepartment DepartmentRepository)
        {
            _mapper = mapper;
            _departmentRepository = DepartmentRepository;
        }
        public void Create(DepartmentDTO dto)
        {
            var department = _mapper.Map<Department>(dto);
            if (department != null)
            {
                department.IsActive = true;
                _departmentRepository.Create(department);
            }           
        }

        public void Delete(int id)
        {
            _departmentRepository.Delete(id);
        }

        public List<DepartmentDTO> GetAll()
        {
            var list = _departmentRepository.GetAll();
            return _mapper.Map<List<DepartmentDTO>>(list);
        }

        public DepartmentDTO GetById(int id)
        {
            var departmentId = _departmentRepository.GetById(id);
            return _mapper.Map<DepartmentDTO>(departmentId);
        }

        public void Update(DepartmentDTO dto)
        {
            var dipartmentId = _departmentRepository.GetById(dto.Id);
            if (dipartmentId != null)
            {
                _mapper.Map(dto, dipartmentId);
                _departmentRepository.Update(dipartmentId);
            }
        }
    }
}
