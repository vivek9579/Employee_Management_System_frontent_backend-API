using Application.DTOs;
using Application.Interface;
using Application.Queries;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;

namespace Application.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IMapper _mapper;
        private readonly IEmployee _employeeRepository;

        public EmployeeServices(IMapper mapper, IEmployee EmployeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = EmployeeRepository;
        }
        public void Create(EmployeeDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            employee.CreatedDate = DateTime.Now;
            employee.IsActive = true;
            _employeeRepository.Add(employee);
        }

        public void Delete(int id)
        {
            _employeeRepository.Delete(id);
        }

        public List<EmployeeDTO> GetAll()
        {
            var employeeList = _employeeRepository.GetAll();
            return _mapper.Map<List<EmployeeDTO>>(employeeList);
        }

        public EmployeeDTO GetById(int id)
        {
            var employeeId = _employeeRepository.GetById(id);
            return _mapper.Map<EmployeeDTO>(employeeId);
        }

        public async Task<EmployeeQuery> GetEmployees(EmployeeQuery query)
        {
            var employees = await _employeeRepository.GetEmployees(query.Search,
                query.Sorting, query.Asc, query.Page, query.PageSize);
            query.Employees = _mapper.Map<List<EmployeeDTO>>(employees.Item1);
            query.TotalRecords = employees.totalRecords;
            return query;
        }

        //public async Task<List<EmployeeDTO>> GetEmployees(EmployeeQuery query)
        //{
        //    var employees = await _employeeRepository.GetEmployees(query.search,
        //        query.sorting,query.asc , query.page, query.pageSize);
        //    return _mapper.Map<List<EmployeeDTO>>(employees);
        //}

        public void Update(EmployeeDTO dto)
        {
            var employee = _employeeRepository.GetById(dto.Id);
            if (employee != null)
            {
             var mapper =    _mapper.Map(dto, employee);
                mapper.UpdatedDate = DateTime.Now;
                _employeeRepository.Update(employee);
            }
        }
    }
}
