// TaskFlow.Shared/Enums/JobStatus.cs

namespace TaskFlow.Shared.Enums
{
    /// <summary>
    /// Represents the possible statuses of a job in the system.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// The job has been created but not yet assigned to a technician.
        /// </summary>
        New,

        /// <summary>
        /// The job has been assigned to a technician.
        /// </summary>
        Assigned,

        /// <summary>
        /// The technician has started working on the job.
        /// </summary>
        InProgress,

        /// <summary>
        /// The job has been successfully completed by the technician.
        /// </summary>
        Completed
    }
}
