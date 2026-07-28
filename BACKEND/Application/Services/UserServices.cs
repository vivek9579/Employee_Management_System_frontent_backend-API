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
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserServices(IMapper mapper , Domain.Interfaces.IUser userRepository
                            , IUserAuthentication userAuthentication
                            , IPasswordHasher<User> passwordHasher
                            , IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userAuthentication = userAuthentication;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
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
            ragister.PasswordHash = _passwordHasher.HashPassword(ragister, dTO.Password);
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
          
            var password = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (password == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // Generate Token
            var refreshToken = new RefreshToken()
            {
                ReToken = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddMinutes(30),
                UserId = user.Id,
                IsRevers = true,
            };
            // Add Database refresh Token
            _refreshTokenRepository.RefreshToken(refreshToken);

            var accessToken = _userAuthentication.UserGenerateToken(user);
            var result = new LoginDTO()
            {
                
                Token = accessToken,
                RefreshToken = refreshToken.ReToken,
                Email = user.Email,
                Role = user.Role
            };
            return result;           

        }

        public LoginDTO RefreshToken(RefreshTokenDTO refreshToken)
        {
            var token = _refreshTokenRepository.GetRefreshToken(refreshToken.RefreshToken);
            if(token == null)
            {
                throw new UnauthorizedAccessException("Inavalid Token");
            }
           
           if(token.ExpiryDate < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Sorry Your'e Token is Expire");
            }
            var user = _userRepository.GetById(token.UserId);
            var accessToken = _userAuthentication.UserGenerateToken(user);

            var result = new LoginDTO
            {
                Token = accessToken,
                RefreshToken = token.ReToken,
                Email = user.Email,
                Role = user.Role
                
            };
            return result;
            
        }
        public void Logout(RefreshTokenDTO refreshToken)
        {
            var token = _refreshTokenRepository.GetRefreshToken(refreshToken.RefreshToken);
            token.IsRevers = false ;
            _refreshTokenRepository.Update(token);
        }
    }
}
