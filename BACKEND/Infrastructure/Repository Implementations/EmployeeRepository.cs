using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository_Implementations
{
    public class EmployeeRepository : IEmployee
    {
        private readonly ManagementDbContext _context;

        public EmployeeRepository(ManagementDbContext context)
        {
            _context = context;
        }
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var employeeId = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (employeeId != null)
            {
                employeeId.IsActive = false;
                _context.SaveChanges();
            }
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.Where(x=>x.IsActive).Include(x => x.Department).ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
    }
}
