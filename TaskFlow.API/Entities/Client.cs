// TaskFlow.API/Entities/Client.cs

using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Entities
{
    /// <summary>
    /// Represents a client or customer for whom jobs are performed.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The unique identifier for the client.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the client (e.g., company or individual).
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The physical address of the client's location for service.
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Contact information for the client, such as a phone number or email.
        /// This field is required.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ContactInfo { get; set; } = string.Empty;
    }
}
