// TaskFlow.API/DTOs/JobCreateDto.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs
{
    /// <summary>
    /// Represents the data required from an Admin to create a new job.
    /// </summary>
    public class JobCreateDto
    {
        /// <summary>
        /// A brief title for the job.
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A detailed description of the work to be done.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// The ID of the client for whom this job is being created.
        /// </summary>
        [Required]
        public int ClientId { get; set; }
    }
}
