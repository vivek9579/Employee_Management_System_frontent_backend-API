using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return _context.Employees.Where(x => x.IsActive).Include(x => x.Department).ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
        }

        public async Task<(List<Employee>, int totalRecords)> GetEmployees(string? search, string? sorting, bool asc, int page, int pageSize)
        {
            var employees = _context.Employees.Include(x=>x.Department)
                .Where(x=>x.IsActive).AsQueryable();
            // Search Start here
            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(x => x.Name.Contains(search)
                            || x.Email.Contains(search)
                            || x.Mobile.ToString().Contains(search)
                            || x.Salary.ToString().Contains(search)
                            || x.Department.Name.Contains(search)
                            );
            }

            // sorting
            switch (sorting)
            {
                case "Id":
                    employees = asc
                        ? employees.OrderBy(x => x.Id)
                        : employees.OrderByDescending(x => x.Id);
                    break;
                default:
                    employees = employees.OrderBy(x => x.Id);
                    break;
            }
            ;
            int totalRecords = await employees.CountAsync();
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);
            var list = await employees.ToListAsync();
            return (list, totalRecords);
        }

        /**
        public async Task<List<Employee>> GetEmployees(string? search, string? sorting, bool asc, 
                    int page, int pageSize)
        {
            var employees = _context.Employees.AsQueryable();
            // Search
            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(x => x.Name.Contains(search)
                            || x.Email.Contains(search));
            }

            // sorting
            switch (sorting)
            {
                case "Id":
                    employees = asc
                        ? employees.OrderBy(x => x.Id)
                        : employees.OrderByDescending(x => x.Id);
                    break;
                    default:
                  employees =  employees .OrderBy(x => x.Id);
                    break;
            };       
           return (await employees.ToListAsync());
        }
        **/
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
    }
}
