// TaskFlow.WebApp/Models/LoginViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebApp.Models
{
    /// <summary>
    /// Represents the data needed for the Login view and form.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
