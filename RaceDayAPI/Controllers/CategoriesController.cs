using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/events/{eventId}/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events/1/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(int eventId)
        {
            return await _context.Categories
                .Where(c => c.EventID == eventId)
                .ToListAsync();
        }

        // POST: api/events/1/categories - Organiser only
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Category>> CreateCategory(int eventId, Category category)
        {
            var raceEvent = await _context.Events.FindAsync(eventId);
            if (raceEvent == null)
            {
                return NotFound();
            }

            category.EventID = eventId;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new { eventId }, category);
        }
    }
}
