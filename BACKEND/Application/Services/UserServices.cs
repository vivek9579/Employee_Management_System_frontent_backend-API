using Application.DTOs;
using Application.Interface;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMapper _mapper;
        private readonly IUser _userRepository;

        public UserServices(IMapper mapper , IUser userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
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
        //public void Login(UserDTO dto)
        //{
        //    _userRepository.Login(dto.Email, dto.Password);
        //}

        public void Ragister(UserDTO dTO)
        {
           var ragister = _mapper.Map<User>(dTO);
            _userRepository.Ragister(ragister);
        }
        public LoginDTO Login(UserDTO dto)
        {
            var user = _userRepository.Login(dto.Email, dto.Password);
            return _mapper.Map<LoginDTO>(user);
        }
    }
}
