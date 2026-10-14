using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <--- THIS FIXES THE "Include" ERROR
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of all events. Public access.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceEvent>>> GetEvents()
        {
            // Changed RaceEvent to Event to match your file
            return await _context.Events.ToListAsync();
        }

        /// <summary>
        /// Creates a new event. Only Organisers can do this.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<RaceEvent>> CreateEvent([FromBody] RaceEvent newEvent)
        {
            // Changed RaceEvent to Event
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return Ok("Event created successfully!");
        }
    }
}

