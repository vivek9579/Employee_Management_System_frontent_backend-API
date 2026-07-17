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
    }
}
