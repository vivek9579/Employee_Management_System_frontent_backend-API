using Application.DTOs;

namespace Application.Interface
{
    public interface IUserServices
    {
        UserDTO GetAll();
        UserDTO GetById(int id);
        void Ragister(UserDTO dTO);
     //   void Login(UserDTO dto);
       LoginDTO Login(UserDTO dto);
        LoginDTO RefreshToken(RefreshTokenDTO refreshToken);
        void Logout(RefreshTokenDTO refreshToken);
    }
}
