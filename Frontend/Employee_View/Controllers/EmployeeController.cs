using Employee_View.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        //private void AddToken()
        //{
        //    var token = HttpContext.Session.GetString("JWTToken");
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token);
        //    }
        //}
        
        public async Task<IActionResult> Index(EmployeeQueryDTO query)
        {
           // AddToken();         
            var employeeList = await _httpClient.GetAsync(
                $"Api/EmployeeApi?" +
                $"search={query.Search}" +
                $"&sorting={query.Sorting}" +
               $"&asc={query.Asc}" +
               $"&page={query.Page}" +
               $"&pagesize={query.PageSize}");
            if(!employeeList.IsSuccessStatusCode)
            {
                var error = await employeeList.Content.ReadFromJsonAsync<ErrorResponse>();
                TempData["eror"] = error?.Message;
                return View(new EmployeeQueryDTO());
            }
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            var list = await employeeList.Content.ReadFromJsonAsync<EmployeeQueryDTO>();
            return View(list);
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
            var response = await _httpClient.PostAsJsonAsync<EmployeeDTO>("Api/EmployeeApi", dto);
            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Index");
            }
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            // ModelState.AddModelError("", error?.Message);
            TempData["error"] = error?.Message;
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
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync<EmployeeDTO>($"Api/EmployeeApi/{dto.Id}", dto);
                if (response.IsSuccessStatusCode)
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
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
