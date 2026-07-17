using Employee_View.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee_View.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClient;

        public DepartmentController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
            //_httpClient = httpClient;
            // _httpClient.BaseAddress = new Uri("https://localhost:44303/");
        }
        public async Task<IActionResult> Index()
        {
            var departmentList = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("Api/DepartmentApi");

            return View(departmentList);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync<DepartmentDTO>("Api/DepartmentApi", dto);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _httpClient.GetFromJsonAsync<DepartmentDTO>($"Api/DepartmentApi/{id}");
            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync<DepartmentDTO>($"Api/DepartmentApi/{dto.Id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Api/DepartmentApi/{id}");
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
