// TaskFlow.WebApp/Models/JobViewModel.cs

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Required for JobStatus enum mapping

namespace TaskFlow.WebApp.Models
{
    // This enum must match the one in your TaskFlow.Shared project.
    // We are duplicating it here for simplicity in the MVC project.
    public enum JobStatus
    {
        New,
        Assigned,
        InProgress,
        Completed
    }

    public class JobViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] // This tells the deserializer how to handle the enum
        public JobStatus Status { get; set; }

        public int ClientId { get; set; }
        public int? AssignedToId { get; set; }

        // --- Navigation Properties for Display ---
        // These will be populated by our API calls.
        [JsonPropertyName("client")]
        public ClientViewModel? Client { get; set; }

        [JsonPropertyName("assignedTo")]
        public UserViewModel? AssignedTo { get; set; }
    }

    // A simple ViewModel to hold user details for display
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
