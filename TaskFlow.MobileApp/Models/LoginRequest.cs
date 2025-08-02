// TaskFlow.MobileApp/Models/LoginRequest.cs

namespace TaskFlow.MobileApp.Models
{
    /// <summary>
    /// Represents the data sent to the API's /login endpoint.
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
