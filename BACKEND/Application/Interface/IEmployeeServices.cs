using Application.DTOs;
using Application.Queries;

namespace Application.Interface
{
    public interface IEmployeeServices
    {
        List<EmployeeDTO> GetAll();
        EmployeeDTO GetById(int id);
        void Create(EmployeeDTO dto);
        void Update(EmployeeDTO dto);
        void Delete(int id);
    // Task<List<EmployeeDTO>> GetEmployees(EmployeeQuery query);
   Task<EmployeeQuery> GetEmployees(EmployeeQuery query);
    }
}
