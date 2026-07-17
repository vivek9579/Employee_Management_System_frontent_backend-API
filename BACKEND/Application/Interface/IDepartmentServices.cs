using Application.DTOs;

namespace Application.Interface
{
    public interface IDepartmentServices
    {
        List<DepartmentDTO> GetAll();
        DepartmentDTO GetById(int id);
        void Create(DepartmentDTO dto);
        void Update(DepartmentDTO dto);
        void Delete(int id);
    }
}
