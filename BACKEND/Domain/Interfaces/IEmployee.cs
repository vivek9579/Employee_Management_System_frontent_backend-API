using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IEmployee
    {
      List<Employee> GetAll();
        Employee GetById(int id);
       void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        //Task<List<Employee>> GetEmployees(
        //    string? search, string? sorting, bool asc );
       Task<(List<Employee>,int totalRecords)> GetEmployees(
            string? search, string? sorting, bool asc,
            int page, int pageSize);
    }
}
