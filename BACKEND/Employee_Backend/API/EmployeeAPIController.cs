using Application.DTOs;
using Application.Interface;
using Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Employee_WebUi.API
{
    [ApiController]
    [Route("Api/[controller]")]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        public EmployeeAPIController(IEmployeeServices EmployeeServices)
        {
            _employeeServices = EmployeeServices;
        }
            [HttpGet]
        //public IActionResult Index()
        //{
        //    var list = _employeeServices.GetAll();
        //    return Ok(list);
        //}
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQuery query)
        {
            var list = await _employeeServices.GetEmployees(query);
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employeeId = _employeeServices.GetById(id);
            return Ok(employeeId);
        }

        [HttpPost]
        public IActionResult Create(EmployeeDTO dto)
        {
            if (ModelState.IsValid)
            {
                _employeeServices.Create(dto);
                return Ok("Employee Create");
            }
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, EmployeeDTO dto)
        {
            var employeeId = _employeeServices.GetById(id);
            dto.Id = id;
            if (ModelState.IsValid)
            {
                _employeeServices.Update(dto);
                return Ok("Employee Updated");
            }
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var employeeId = _employeeServices.GetById(id);
            if (employeeId != null)
            {
                _employeeServices.Delete(id);
                return Ok("Employee Deleted");
            }
            return Ok(employeeId);
        }
    }
}
