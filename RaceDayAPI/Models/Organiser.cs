using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrganisationName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ContactNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

