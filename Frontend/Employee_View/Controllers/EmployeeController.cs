using Employee_View.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee_View.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClient;

        public EmployeeController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
            //_httpClient = httpClient;
            // _httpClient.BaseAddress = new Uri("https://localhost:44303/");
        }
        public async Task<IActionResult> Index(EmployeeQueryDTO query)
        {
            var employeeList = await _httpClient.GetFromJsonAsync<EmployeeQueryDTO>(
                $"Api/EmployeeApi?" + 
                $"search={query.Search}" + 
                $"&sorting={query.Sorting}" +
               $"&asc={query.Asc}" + 
               $"&page={query.Page}" + 
               $"&pagesize={query.PageSize}");
            //foreach (var item in employeeList)
            //{
            //    item.sorting = query.sorting;
            //    item.asc = query.asc;
            //}
            return View(employeeList);
        }

        public async Task<IActionResult> Create()
        {
            var employee = new EmployeeDTO();
            employee.Departments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("Api/DepartmentApi");
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDTO dto)
        {
            if(ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync<EmployeeDTO>("Api/EmployeeApi",dto);
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            dto.Departments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("Api/DepartmentApi");
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _httpClient.GetFromJsonAsync<EmployeeDTO>($"Api/EmployeeApi/{id}");
            employee.Id = id;
             employee.Departments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("Api/DepartmentApi");
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDTO dto)
        {
            if(ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync<EmployeeDTO>($"Api/EmployeeApi/{dto.Id}", dto);
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            dto.Departments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("Api/DepartmentApi");
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Api/EmployeeApi/{id}");
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
