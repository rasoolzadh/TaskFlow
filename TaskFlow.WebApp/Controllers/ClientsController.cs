// TaskFlow.WebApp/Controllers/ClientsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TaskFlow.WebApp.Models;

namespace TaskFlow.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Clients
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/clients");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(clients);
            }

            ViewBag.ErrorMessage = $"Error retrieving clients. Status: {response.StatusCode}";
            return View(new List<ClientViewModel>());
        }

        // GET: /Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel client)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                var token = User.FindFirstValue("jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsJsonAsync("/api/clients", client);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, $"Failed to create client. API responded with status: {response.StatusCode}");
            }
            return View(client);
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var client = JsonSerializer.Deserialize<ClientViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(client);
            }

            return NotFound();
        }

        // POST: /Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel client)
        {
            if (id != client.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                var token = User.FindFirstValue("jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsJsonAsync($"/api/clients/{id}", client);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, $"Failed to update client. API responded with status: {response.StatusCode}");
            }
            return View(client);
        }

        // --- NEW ACTION METHOD ---
        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/clients/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var client = JsonSerializer.Deserialize<ClientViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(client);
            }

            return NotFound();
        }

        // GET: /Clients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var client = JsonSerializer.Deserialize<ClientViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(client);
            }

            return NotFound();
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.DeleteAsync($"/api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = $"Failed to delete client. API responded with status: {response.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
