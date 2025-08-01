// TaskFlow.API/Controllers/ClientsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.Entities;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // <-- IMPORTANT: Secures the entire controller
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of all clients. (Admin Only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Gets a specific client by their ID. (Admin Only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            return Ok(client);
        }

        /// <summary>
        /// Creates a new client. (Admin Only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Returns a 201 Created response with a link to the new resource
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        /// <summary>
        /// Updates an existing client. (Admin Only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest("Client ID mismatch.");
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clients.Any(e => e.Id == id))
                {
                    return NotFound("Client not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Indicates success with no content to return
        }

        /// <summary>
        /// Deletes a client. (Admin Only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent(); // Indicates success with no content to return
        }
    }
}
