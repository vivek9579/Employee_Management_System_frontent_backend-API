using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.APIControllers
{
    public class DepartmentController : Controller
    {
        private readonly HttpClient _httpClient;

        public DepartmentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7135/");
        }
        public async Task<IActionResult> Index()
        {
            var depatments = await _httpClient.GetFromJsonAsync<List<DepartmentDTO>>("api/DepartmentAPI");
            return View(depatments);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/DepartmentAPI", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _httpClient.GetFromJsonAsync<DepartmentDTO>($"api/DepartmentAPI/{id}");
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/DepartmentAPI/{dto.Id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/DepartmentApi/{id}");
            return RedirectToAction("Index");
        }
    }
}
