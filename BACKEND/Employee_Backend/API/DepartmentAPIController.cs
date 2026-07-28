using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Employee_WebUi.API
{
    [ApiController]
    [Route("Api/[Controller]")]
    [Authorize]
    public class DepartmentAPIController : ControllerBase
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentAPIController(IDepartmentServices DepartmentServices)
        {
            _departmentServices = DepartmentServices;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
           var departmentList = _departmentServices.GetAll();
            return Ok(departmentList);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var departmentId = _departmentServices.GetById(id);
            return Ok(departmentId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(DepartmentDTO dto)
        {
            if(ModelState.IsValid)
            {
                _departmentServices.Create(dto);
                return Ok("Department Created");
            }
            return Ok(dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, DepartmentDTO dto)
        {
            var departmentId = _departmentServices.GetById(id);
            dto.Id = id;
            if(ModelState.IsValid)
            {
                _departmentServices.Update(dto);
                return Ok("Department Updated");
            }
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _departmentServices.Delete(id);
            return Ok("Department Deleted");

        }
    }
}
