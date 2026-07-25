using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IUserAuthentication
    {
        string UserGenerateToken(User user);
    }
}
