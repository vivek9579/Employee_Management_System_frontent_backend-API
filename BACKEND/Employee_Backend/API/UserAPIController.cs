using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Employee_WebUi.API
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserAPIController(IUserServices UserServices)
        {
            _userServices = UserServices;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var list = _userServices.GetAll();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = _userServices.GetById(id);
            return Ok(userId);
        }

        [HttpPost("Ragister")]
        public IActionResult Ragister(UserDTO dto)
        {
            if (ModelState.IsValid)
            {
                _userServices.Ragister(dto);
                return Ok("User Ragistered");
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public IActionResult Login(UserDTO dto)
        {
            var user = _userServices.Login(dto);
            if (user == null)
            {
                return Unauthorized(user);
            }
            return Ok(user);
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(RefreshTokenDTO dto)
        {
            try
            {
                var refreshtoken = _userServices.RefreshToken(dto);
                return Ok(refreshtoken);
            }
            catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("Logout")]
        public IActionResult Logout(RefreshTokenDTO dto)
        {
            _userServices.Logout(dto);
            return Ok("Your'e Logout this website");
        }
    }
}
