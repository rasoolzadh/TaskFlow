// TaskFlow.WebApp/Models/LoginResult.cs

using System.Text.Json.Serialization;

namespace TaskFlow.WebApp.Models
{
    /// <summary>
    /// Represents the successful login response from the API, containing the JWT.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// The JSON Web Token provided by the API.
        /// The JsonPropertyName attribute ensures it correctly maps from the JSON response.
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
