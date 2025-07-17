using Microsoft.AspNetCore.Mvc;
using Nexus.Core.Dtos;
using System.Text.Json;

namespace Nexus.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            var client = _httpClientFactory.CreateClient("NexusApiClient");
            var response = await client.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                //ReadAsString waits for the content property to fully load
                var jsonString = await response.Content.ReadAsStringAsync();
                var tokenObj = JsonDocument.Parse(jsonString);
                var token = tokenObj.RootElement.GetProperty("token").GetString();

                HttpContext.Session.SetString("JWToken", token);
                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginDto);
        }
    }
}
