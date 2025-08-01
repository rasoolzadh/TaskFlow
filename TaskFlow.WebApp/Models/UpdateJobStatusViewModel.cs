// TaskFlow.WebApp/Models/UpdateJobStatusViewModel.cs

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebApp.Models
{
    public class UpdateJobStatusViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "New Status")]
        public JobStatus Status { get; set; }

        public SelectList? Statuses { get; set; }
    }
}
