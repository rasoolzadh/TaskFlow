// TaskFlow.WebApp/Controllers/AccountController.cs

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TaskFlow.WebApp.Models;

namespace TaskFlow.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var response = await httpClient.PostAsJsonAsync("/api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (string.IsNullOrEmpty(loginResult?.Token))
                {
                    ModelState.AddModelError(string.Empty, "Invalid token received.");
                    return View(model);
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(loginResult.Token);

                var claims = jwtToken.Claims.ToList();
                claims.Add(new Claim("jwt", loginResult.Token));

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    "TaskFlowCookieAuth",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType
                );

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("TaskFlowCookieAuth", claimsPrincipal);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("TaskFlowCookieAuth");

            // --- THE FIX ---
            // Redirect to the Login page after logging out.
            return RedirectToAction("Login", "Account");
        }
    }
}
