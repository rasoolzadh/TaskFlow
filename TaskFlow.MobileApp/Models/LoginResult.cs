// TaskFlow.MobileApp/Models/LoginResult.cs

using System.Text.Json.Serialization;

namespace TaskFlow.MobileApp.Models
{
    public class LoginResult
    {
        [JsonPropertyName("token")]
        // FIX: Initialize the property to satisfy the compiler's null-safety check.
        public string Token { get; set; } = string.Empty;
    }
}
