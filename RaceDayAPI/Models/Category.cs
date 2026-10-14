using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        [ForeignKey("EventID")]
        public RaceEvent? Event { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public int MinAge { get; set; }

        public int? MaxAge { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; } = "Open";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EntryFee { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        public int CurrentEnrollments { get; set; } = 0;
    }
}

