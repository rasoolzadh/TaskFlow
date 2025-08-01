// TaskFlow.WebApp/Controllers/JobsController.cs

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
    public class JobsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public JobsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Jobs
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/jobs");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jobs = JsonSerializer.Deserialize<List<JobViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(jobs);
            }

            ViewBag.ErrorMessage = "Could not retrieve jobs from the API.";
            return View(new List<JobViewModel>());
        }

        // GET: /Jobs/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new JobCreateViewModel
            {
                Clients = await GetClientsSelectList()
            };
            return View(viewModel);
        }

        // POST: /Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                var token = User.FindFirstValue("jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsJsonAsync("/api/jobs", viewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, $"Failed to create job. API responded with status: {response.StatusCode}");
            }

            viewModel.Clients = await GetClientsSelectList();
            return View(viewModel);
        }

        // GET: /Jobs/Assign/5
        public async Task<IActionResult> Assign(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jobResponse = await httpClient.GetAsync($"/api/jobs/{id}");
            if (!jobResponse.IsSuccessStatusCode) return NotFound();
            var job = await jobResponse.Content.ReadFromJsonAsync<JobViewModel>();

            var technicians = await GetTechniciansList();

            var viewModel = new AssignJobViewModel
            {
                JobId = job.Id,
                JobTitle = job.Title,
                JobDescription = job.Description,
                ClientName = job.Client?.Name ?? "N/A",
                Technicians = new SelectList(technicians, "Id", "Name")
            };

            return View(viewModel);
        }

        // POST: /Jobs/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                var token = User.FindFirstValue("jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsync($"/api/jobs/{viewModel.JobId}/assign/{viewModel.TechnicianId}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, $"Failed to assign job. API responded with status: {response.StatusCode}");
            }

            viewModel.Technicians = new SelectList(await GetTechniciansList(), "Id", "Name");
            return View(viewModel);
        }

        // --- NEW ACTION METHOD ---
        // GET: /Jobs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/jobs/{id}");

            if (response.IsSuccessStatusCode)
            {
                var job = await response.Content.ReadFromJsonAsync<JobViewModel>();
                return View(job);
            }

            return NotFound();
        }

        // --- HELPER METHODS ---
        private async Task<SelectList?> GetClientsSelectList()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/clients");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new SelectList(clients, "Id", "Name");
        }

        private async Task<IEnumerable<UserViewModel>> GetTechniciansList()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/users/technicians");
            if (!response.IsSuccessStatusCode) return new List<UserViewModel>();

            return await response.Content.ReadFromJsonAsync<List<UserViewModel>>() ?? new List<UserViewModel>();
        }
    }
}
