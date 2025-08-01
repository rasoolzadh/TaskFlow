// TaskFlow.WebApp/Controllers/UsersController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TaskFlow.WebApp.Models;

namespace TaskFlow.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/users");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<UserViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(users);
            }

            ViewBag.ErrorMessage = "Could not retrieve users from the API.";
            return View(new List<UserViewModel>());
        }

        // GET: /Users/Create
        public IActionResult Create()
        {
            var viewModel = new RegisterViewModel
            {
                Roles = new SelectList(new[] { "Admin", "Technician" })
            };
            return View(viewModel);
        }

        // --- NEW ACTION METHOD ---
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                // Note: The register endpoint on our API is public, so it does not require an auth token.

                // We send the model directly to the API's register endpoint
                var response = await httpClient.PostAsJsonAsync("/api/auth/register", model);

                if (response.IsSuccessStatusCode)
                {
                    // After successfully creating the user, redirect to the list of all users.
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the API returns an error (e.g., email already exists), display it.
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Failed to create user. API responded with: {errorContent}");
                }
            }

            // If we get here, something failed. We must re-populate the Roles dropdown.
            model.Roles = new SelectList(new[] { "Admin", "Technician" });
            return View(model);
        }
    }
}
