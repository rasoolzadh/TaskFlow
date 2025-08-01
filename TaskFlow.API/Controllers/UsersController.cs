// TaskFlow.API/Controllers/UsersController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.Entities;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- NEW ACTION METHOD ---
        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/technicians
        [HttpGet("technicians")]
        public async Task<ActionResult<IEnumerable<User>>> GetTechnicians()
        {
            var technicians = await _context.Users
                .Where(u => u.Role == "Technician")
                .ToListAsync();

            return Ok(technicians);
        }
    }
}
