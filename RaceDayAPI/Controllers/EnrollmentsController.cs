using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrollmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/enrollments/my - Participant's own enrollments
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetMyEnrollments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);

            if (participant == null)
            {
                return BadRequest(new { message = "User is not a participant" });
            }

            return await _context.Enrollments
                .Where(e => e.ParticipantID == participant.ParticipantID)
                .Include(e => e.Category)
                .ThenInclude(c => c!.Event)
                .ToListAsync();
        }

        // POST: api/enrollments - Participant enrolls in a category
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<Enrollment>> Enroll([FromBody] EnrollRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.UserID == userId);

            if (participant == null)
            {
                return BadRequest(new { message = "User is not a participant" });
            }

            var category = await _context.Categories.FindAsync(request.CategoryID);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            // Check if already enrolled
            var exists = await _context.Enrollments
                .AnyAsync(e => e.ParticipantID == participant.ParticipantID && e.CategoryID == request.CategoryID);

            if (exists)
            {
                return BadRequest(new { message = "Already enrolled in this category" });
            }

            var enrollment = new Enrollment
            {
                ParticipantID = participant.ParticipantID,
                CategoryID = request.CategoryID,
                PaymentAmount = category.EntryFee,
                PaymentStatus = "Pending",
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyEnrollments), new { }, enrollment);
        }
    }

    public class EnrollRequest
    {
        public int CategoryID { get; set; }
    }
}
