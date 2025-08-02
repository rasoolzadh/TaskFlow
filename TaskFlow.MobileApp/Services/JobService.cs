// TaskFlow.MobileApp/Services/JobService.cs

using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskFlow.MobileApp.Models;

namespace TaskFlow.MobileApp.Services
{
    public class JobService
    {
        private readonly HttpClient _httpClient;

        public JobService()
        {
            var handlerService = new HttpsClientHandlerService();
            _httpClient = new HttpClient(handlerService.GetPlatformMessageHandler());

            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7258"
                : "https://localhost:7258";
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<List<Job>> GetMyJobsAsync()
        {
            try
            {
                var token = await SecureStorage.Default.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    return new List<Job>();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jobs = await _httpClient.GetFromJsonAsync<List<Job>>("/api/jobs/my-jobs");
                return jobs ?? new List<Job>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching jobs: {ex.Message}");
            }

            return new List<Job>();
        }

        // --- NEW METHOD ---
        /// <summary>
        /// Updates the status of a specific job.
        /// </summary>
        /// <param name="jobId">The ID of the job to update.</param>
        /// <param name="newStatus">The new status for the job.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateJobStatusAsync(int jobId, JobStatus newStatus)
        {
            try
            {
                var token = await SecureStorage.Default.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // The API expects a simple JSON object like: { "status": "InProgress" }
                var payload = new { status = newStatus };

                var response = await _httpClient.PutAsJsonAsync($"/api/jobs/{jobId}/status", payload);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating job status: {ex.Message}");
                return false;
            }
        }
    }
}
