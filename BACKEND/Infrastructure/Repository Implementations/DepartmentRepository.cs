using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repository_Implementations
{
    public class DepartmentRepository : IDepartment
    {
        private readonly ManagementDbContext _context;

        public DepartmentRepository(ManagementDbContext context)
        {
            _context = context;
        }
        public void Create(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var departmentId = _context.Departments.FirstOrDefault(x => x.Id == id);
            if(departmentId != null)
            {
                departmentId.IsActive = false;
                _context.SaveChanges();
            }
        }

        public List<Department> GetAll()
        {
            return _context.Departments.Where(x=>x.IsActive).ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.FirstOrDefault(x => x.Id == id);

        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }
    }
}
