using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2025_Project3_krhanson3.Models
{
    public class Tweets
    {
        [Key]
        public int TweetId { get; set; }

        // Foreign Key to Actor
        public int ActorId { get; set; }

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public double Sentiment { get; set; }

        // Navigation property
        public Actors Actor { get; set; }
    }
}
