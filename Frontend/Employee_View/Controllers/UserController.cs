using Employee_View.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
                    HttpContext.Session.SetString("JWTToken", read.Token);
                    HttpContext.Session.SetString("RefreshToken", read.RefreshToken);
                    var claims = new List<Claim>()
                {
                       new Claim(ClaimTypes.Email , read.Email),
                      new Claim(ClaimTypes.Role , read.Role),

                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults
                                                    .AuthenticationScheme, new ClaimsPrincipal(identity));
                    if (read.Role == "User")
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    if (read.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                }
            }
            return View(dto);
        }


        public async Task<IActionResult> logout()
        {
            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var dto = new RefrenceTokenDTO
            {
                RefreshToken = HttpContext.Session.GetString("RefreshToken")
            };
            await _httpClient.PatchAsJsonAsync("Api/UserAPI/Logout", dto);
            HttpContext.Session.Clear();
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
