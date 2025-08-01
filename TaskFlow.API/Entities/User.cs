// TaskFlow.API/Entities/User.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Entities
{
    /// <summary>
    /// Represents a user in the system.
    /// Can be an Administrator or a Technician.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The full name of the user.
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address, used for login.
        /// This field is required and must be unique.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The hashed password for the user.
        /// The actual password is never stored directly.
        /// This field is required.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// The role assigned to the user (e.g., "Admin", "Technician").
        /// This determines the user's permissions within the application.
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
    }
}
