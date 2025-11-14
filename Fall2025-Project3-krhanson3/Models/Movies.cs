using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Fall2025_Project3_krhanson3.Models
{
    public class Movies
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Genre { get; set; }

        public int ReleaseYear { get; set; }

        [Url]
        public string? IMDBUrl { get; set; }

        public string? IMDBUrlSRI { get; set; }

        // Stored in DB
        public byte[]? Poster { get; set; }

        public string? PosterSRI { get; set; }

        // Used only for uploads
        [NotMapped]
        public IFormFile? PosterFile { get; set; }

        public ICollection<MovieActor>? MovieActors { get; set; }

        //Reviews
        public ICollection<Reviews> Reviews { get; set; }

        public double SentimentAverage { get; set; }
    }
}
