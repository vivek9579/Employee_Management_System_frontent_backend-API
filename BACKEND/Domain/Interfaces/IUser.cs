using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IUser
    {
        List<User> GetAll();
        User GetById(int id);
        void Ragister(User user);
        User? Login(string email, string password);
    }
}
