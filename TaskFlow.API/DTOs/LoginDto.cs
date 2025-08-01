// TaskFlow.API/DTOs/LoginDto.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs
{
    /// <summary>
    /// Represents the data required for a user to log in.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
