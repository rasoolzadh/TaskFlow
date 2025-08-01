// TaskFlow.API/DTOs/JobStatusUpdateDto.cs

using System.ComponentModel.DataAnnotations;
using TaskFlow.API.Entities;
using TaskFlow.Shared.Enums; // <-- Remember to add this using statement

namespace TaskFlow.API.DTOs
{
    /// <summary>
    /// Represents the data required from a Technician to update a job's status.
    /// </summary>
    public class JobStatusUpdateDto
    {
        /// <summary>
        
        /// </summary>
        [Required]
        public JobStatus Status { get; set; }
    }
}
