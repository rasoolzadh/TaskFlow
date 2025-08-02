// TaskFlow.MobileApp/Models/Client.cs

namespace TaskFlow.MobileApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        // FIX: Initialize properties with a default value to satisfy the compiler.
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
