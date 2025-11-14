using System.Collections.Generic;

namespace Fall2025_Project3_krhanson3.Models.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string? IMDBUrl { get; set; }
        public byte[]? Poster { get; set; }

        // Actors in the movie
        public List<ActorInfo> Actors { get; set; } = new();

        public double? SentimentAverage { get; set; }

        public List<ReviewsInfo> Review { get; set; } = new();

    }

    public class ActorInfo
    {
        public int ActorId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ReviewsInfo
    {
        public int ReviewId { get; set; }
        public int MovieId { get; set; }
        public Movies Movie { get; set; }
        public string User { get; set; }
        public string Text { get; set; } = string.Empty;
        public double Sentiment { get; set; }
    }

}
