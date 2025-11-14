using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2025_Project3_krhanson3.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }

        // Foreign Key to Actor
        public int MovieId { get; set; }

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public double Sentiment { get; set; }

        // Navigation property
        public Movies Movie { get; set; }
    }
}
