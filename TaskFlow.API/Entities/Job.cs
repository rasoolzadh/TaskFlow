// TaskFlow.API/Entities/Job.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.Shared.Enums;

namespace TaskFlow.API.Entities
{
    /// <summary>
    /// Represents a single job or task to be performed for a client.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// The unique identifier for the job.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// A brief title for the job.
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A detailed description of the work to be done.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// The current status of the job (e.g., New, Assigned, InProgress, Completed).
        /// Defaults to 'New' when a job is created.
        /// </summary>
        [Required]
        public JobStatus Status { get; set; } = JobStatus.New;

        // --- Foreign Key & Navigation Property for Client ---

        /// <summary>
        /// The foreign key referencing the client for whom this job is being done.
        /// </summary>
        [Required]
        public int ClientId { get; set; }

        /// <summary>
        /// The navigation property to the associated Client entity.
        /// </summary>
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        // --- Foreign Key & Navigation Property for User (Technician) ---

        /// <summary>
        /// The foreign key referencing the user (technician) assigned to this job.
        /// This can be null if the job is not yet assigned.
        /// </summary>
        public int? AssignedToId { get; set; }

        /// <summary>
        /// The navigation property to the associated User (Technician) entity.
        /// </summary>
        [ForeignKey("AssignedToId")]
        public User? AssignedTo { get; set; }
    }
}
