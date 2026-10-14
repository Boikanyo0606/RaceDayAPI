using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <--- THIS FIXES THE "Include" ERROR
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all results for a specific event.
        /// </summary>
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<RaceResult>>> GetEventResults(int eventId)
        {
            // Changed RaceResult to Result
            return await _context.Results
                .Include(r => r.Enrollment) // Include works now because of the using statement above!
                .Where(r => r.Enrollment.Category.EventID == eventId)
                .ToListAsync();
        }

        /// <summary>
        /// Records a new result. Only Organisers can do this.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<RaceResult>> RecordResult([FromBody] RaceResult newResult)
        {
            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();
            return Ok("Result recorded!");
        }
    }
}