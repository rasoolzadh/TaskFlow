// TaskFlow.API/Data/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskFlow.API.Entities;

namespace TaskFlow.API.Data
{
    /// <summary>
    /// Represents the database context for the application,
    /// managing the connection and mapping entities to the database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for Users. This will be mapped to the "Users" table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Clients. This will be mapped to the "Clients" table in the database.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Jobs. This will be mapped to the "Jobs" table in the database.
        /// </summary>
        public DbSet<Job> Jobs { get; set; }
    }
}
