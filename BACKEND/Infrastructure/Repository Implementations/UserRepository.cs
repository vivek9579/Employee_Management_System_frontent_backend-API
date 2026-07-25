using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repository_Implementations
{
    public class UserRepository : IUser
    {
        private readonly ManagementDbContext _context;

        public UserRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
           return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User Login(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public void Ragister(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
