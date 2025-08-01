// TaskFlow.WebApp/Controllers/TechnicianController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TaskFlow.WebApp.Models;

namespace TaskFlow.WebApp.Controllers
{
    [Authorize(Roles = "Technician")]
    public class TechnicianController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TechnicianController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Technician/MyJobs
        public async Task<IActionResult> MyJobs()
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/jobs/my-jobs");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jobs = JsonSerializer.Deserialize<List<JobViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(jobs);
            }

            ViewBag.ErrorMessage = "Could not retrieve your assigned jobs from the API.";
            return View(new List<JobViewModel>());
        }

        // GET: /Technician/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/jobs/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var job = await response.Content.ReadFromJsonAsync<JobViewModel>();

            // Security Check: Make sure the technician owns this job
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (job.AssignedToId.ToString() != currentUserId)
            {
                return Forbid(); // User is not authorized to see this job
            }

            var availableStatuses = new[]
            {
                new { Id = JobStatus.InProgress, Name = "In Progress" },
                new { Id = JobStatus.Completed, Name = "Completed" }
            };

            var viewModel = new UpdateJobStatusViewModel
            {
                JobId = job.Id,
                JobTitle = job.Title,
                ClientName = job.Client?.Name ?? "N/A",
                Status = job.Status,
                Statuses = new SelectList(availableStatuses, "Id", "Name")
            };

            return View(viewModel);
        }

        // POST: /Technician/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateJobStatusViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
                var token = User.FindFirstValue("jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var statusUpdatePayload = new { status = viewModel.Status };
                var response = await httpClient.PutAsJsonAsync($"/api/jobs/{viewModel.JobId}/status", statusUpdatePayload);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(MyJobs));
                }

                ModelState.AddModelError(string.Empty, $"Failed to update status. API responded with: {response.StatusCode}");
            }

            var availableStatuses = new[]
            {
                new { Id = JobStatus.InProgress, Name = "In Progress" },
                new { Id = JobStatus.Completed, Name = "Completed" }
            };
            viewModel.Statuses = new SelectList(availableStatuses, "Id", "Name");
            return View(viewModel);
        }

        // --- NEW ACTION METHOD ---
        // GET: /Technician/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("TaskFlowAPI");
            var token = User.FindFirstValue("jwt");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"/api/jobs/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var job = await response.Content.ReadFromJsonAsync<JobViewModel>();

            // Security Check: Make sure the technician owns this job
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (job.AssignedToId.ToString() != currentUserId)
            {
                return Forbid(); // User is not authorized to see this job
            }

            return View(job);
        }
    }
}
