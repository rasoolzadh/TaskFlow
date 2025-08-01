// TaskFlow.API/Controllers/JobsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskFlow.API.Data;
using TaskFlow.API.DTOs;
using TaskFlow.API.Entities;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <-- Base authorization for the whole controller
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ENDPOINT FOR ADMINS: Create a new job
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Job>> CreateJob(JobCreateDto jobDto)
        {
            var clientExists = await _context.Clients.AnyAsync(c => c.Id == jobDto.ClientId);
            if (!clientExists)
            {
                return BadRequest("The specified client does not exist.");
            }

            var job = new Job
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                ClientId = jobDto.ClientId,
                Status = Shared.Enums.JobStatus.New // Default status
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }

        // ENDPOINT FOR ADMINS: Assign a job to a technician
        [HttpPut("{id}/assign/{technicianId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignJob(int id, int technicianId)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound("Job not found.");
            }

            var technician = await _context.Users.FirstOrDefaultAsync(u => u.Id == technicianId && u.Role == "Technician");
            if (technician == null)
            {
                return BadRequest("Invalid Technician ID or the user is not a Technician.");
            }

            job.AssignedToId = technicianId;
            job.Status = Shared.Enums.JobStatus.Assigned;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ENDPOINT FOR TECHNICIANS: Get jobs assigned to the current user
        [HttpGet("my-jobs")]
        [Authorize(Roles = "Technician")]
        public async Task<ActionResult<IEnumerable<Job>>> GetMyJobs()
        {
            // Get the current user's ID from their token claims
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var jobs = await _context.Jobs
                .Where(j => j.AssignedToId.ToString() == currentUserId)
                .Include(j => j.Client) // Include client details
                .ToListAsync();

            return Ok(jobs);
        }

        // ENDPOINT FOR TECHNICIANS: Update the status of an assigned job
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> UpdateJobStatus(int id, JobStatusUpdateDto statusDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound("Job not found.");
            }

            // Security Check: Ensure the technician is updating their own job
            if (job.AssignedToId.ToString() != currentUserId)
            {
                return Forbid("You are not authorized to update this job.");
            }

            job.Status = statusDto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // SHARED ENDPOINT (ADMINS & TECHNICIANS): Get a single job by its ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Client) // Include client details
                .Include(j => j.AssignedTo) // Include technician details
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            // If the user is a technician, ensure they can only see their own jobs
            if (User.IsInRole("Technician"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (job.AssignedToId.ToString() != currentUserId)
                {
                    return Forbid("You are not authorized to view this job.");
                }
            }

            return Ok(job);
        }

        // ENDPOINT FOR ADMINS: Get a list of all jobs
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Client)
                .Include(j => j.AssignedTo)
                .ToListAsync();

            return Ok(jobs);
        }
    }
}
