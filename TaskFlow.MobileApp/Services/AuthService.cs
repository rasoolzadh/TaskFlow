// TaskFlow.MobileApp/Services/AuthService.cs

using System.Net.Http.Json;
using System.Text.Json;
using TaskFlow.MobileApp.Models;

namespace TaskFlow.MobileApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public AuthService()
        {
            // --- FIX: Use the custom handler to bypass SSL validation ---
            var handlerService = new HttpsClientHandlerService();
            _httpClient = new HttpClient(handlerService.GetPlatformMessageHandler());

            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7258"
                : "https://localhost:7258";
            _httpClient.BaseAddress = new Uri(baseAddress);

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<string?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResult>(content, _serializerOptions);
                    return loginResult?.Token;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during login: {ex.Message}");
            }

            return null;
        }
    }
}
