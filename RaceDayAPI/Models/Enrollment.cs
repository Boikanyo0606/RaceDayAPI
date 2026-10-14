using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        [Required]
        public int ParticipantID { get; set; }

        [ForeignKey("ParticipantID")]
        public Participant? Participant { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? BibNumber { get; set; }
    }
}

