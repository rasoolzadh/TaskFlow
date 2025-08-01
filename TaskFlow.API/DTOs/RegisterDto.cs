// TaskFlow.API/DTOs/RegisterDto.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs
{
    /// <summary>
    /// Represents the data required to register a new user.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// The full name of the user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address. This will be their username for logging in.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's chosen password.
        /// It must be at least 8 characters long.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The role for the new user.
        /// Must be either "Admin" or "Technician".
        /// </summary>
        [Required]
        [RegularExpression("Admin|Technician", ErrorMessage = "Role must be 'Admin' or 'Technician'")]
        public string Role { get; set; } = string.Empty;
    }
}
