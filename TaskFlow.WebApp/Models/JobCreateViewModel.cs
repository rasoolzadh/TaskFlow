// TaskFlow.WebApp/Models/JobCreateViewModel.cs

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebApp.Models
{
    /// <summary>
    /// ViewModel for the Create Job page.
    /// </summary>
    public class JobCreateViewModel
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a client.")]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        // This property will hold the list of clients for the dropdown menu.
        // It is not sent to the API.
        public SelectList? Clients { get; set; }
    }
}
