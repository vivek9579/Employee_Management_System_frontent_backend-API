using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.APIControllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
       // private readonly IDepartment _departmentServices;

        public EmployeeController(HttpClient httpClient
                        )
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://localhost:7135/");
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _httpClient.GetFromJsonAsync<List<EmployeeDTO>>(
                "api/EmployeeApi");
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            var dto = new EmployeeDTO();
            dto.Departments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>
   (
       "api/DepartmentAPI"
   );
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/EmployeeApi", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _httpClient.GetFromJsonAsync<EmployeeDTO>($"api/EmployeeApi/{id}");
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/EmployeeApi/{dto.Id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/EmployeeApi/{id}");
            return RedirectToAction("Index");
        }
    }
}
