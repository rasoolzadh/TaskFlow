// TaskFlow.WebApp/Models/AssignJobViewModel.cs

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebApp.Models
{
    public class AssignJobViewModel
    {
        // Details of the job to be displayed on the page
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? JobDescription { get; set; }
        public string ClientName { get; set; } = string.Empty;

        // The ID of the selected technician from the form
        [Required(ErrorMessage = "Please select a technician.")]
        [Display(Name = "Assign To")]
        public int TechnicianId { get; set; }

        // The list of available technicians for the dropdown
        public SelectList? Technicians { get; set; }
    }
}
