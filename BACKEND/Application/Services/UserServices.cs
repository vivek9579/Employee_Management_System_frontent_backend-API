using Application.DTOs;
using Application.Interface;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMapper _mapper;
        private readonly Domain.Interfaces.IUser _userRepository;
        private readonly IUserAuthentication _userAuthentication;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserServices(IMapper mapper , Domain.Interfaces.IUser userRepository
                            , IUserAuthentication userAuthentication
                            , IPasswordHasher<User> passwordHasher)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userAuthentication = userAuthentication;
            _passwordHasher = passwordHasher;
        }

        public UserDTO GetAll()
        {
            var list = _userRepository.GetAll();
            return _mapper.Map<UserDTO>(list);
        }

        public UserDTO GetById(int id)
        {
            var userId = _userRepository.GetById(id);
            return _mapper.Map<UserDTO>(userId);
        }
        
        public void Ragister(UserDTO dTO)
        {
           var ragister = _mapper.Map<User>(dTO);
            ragister.Password = _passwordHasher.HashPassword(ragister, dTO.Password);
            _userRepository.Ragister(ragister);
        }
        public LoginDTO Login(UserDTO dto)
        {
            var user = _userRepository.Login(dto.Email);
          //  return _mapper.Map<LoginDTO>(user);
            if (user == null)
            {
                return null;
            }
            var password = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (password == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = _userAuthentication.UserGenerateToken(user);

            var result = new LoginDTO()
            {
                Token = token,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
            return result;
            

        }
    }
}
