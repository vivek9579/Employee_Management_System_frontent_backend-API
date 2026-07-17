using Employee_View.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Employee_View.Controllers
{
    public class UserController : Controller
    {
        //private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
            //_httpClient = httpClient;
            // _httpClient.BaseAddress = new Uri("https://localhost:44303/");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Api/UserAPI/Login",
                   dto, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var read = await response.Content.ReadFromJsonAsync<LoginDTO>(cancellationToken);
                    var claims = new List<Claim>()
                {
                       new Claim(ClaimTypes.Email , read.Email)
                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults
                                                    .AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Department");
                }
                
            }
            
            return View(dto);
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Ragister()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Ragister(UserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync<UserDTO>("Api/UserAPI/Ragister", dto);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("Login");
                }
            }
            return View(dto);
        }
    }
}
