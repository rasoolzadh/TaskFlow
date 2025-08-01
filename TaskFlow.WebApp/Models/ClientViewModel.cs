// TaskFlow.WebApp/Models/ClientViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebApp.Models
{
    /// <summary>
    /// Represents a Client for display and editing in the views.
    /// </summary>
    public class ClientViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Contact Info")]
        [StringLength(100)]
        public string ContactInfo { get; set; } = string.Empty;
    }
}
