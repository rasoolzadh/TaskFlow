// TaskFlow.MobileApp/Models/Job.cs

using System.Text.Json.Serialization;

namespace TaskFlow.MobileApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum JobStatus
    {
        New,
        Assigned,
        InProgress,
        Completed
    }

    public class Job
    {
        public int Id { get; set; }

        // FIX: Initialize string properties to avoid null warnings.
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public JobStatus Status { get; set; }
        public int ClientId { get; set; }

        // FIX: Initialize the Client property to a new Client object.
        public Client Client { get; set; } = new();
    }
}
