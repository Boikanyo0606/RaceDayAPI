using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class RaceResult
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        public int EnrollmentID { get; set; }

        [ForeignKey("EnrollmentID")]
        public Enrollment? Enrollment { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        public int? CategoryPosition { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.Now;
    }
}

