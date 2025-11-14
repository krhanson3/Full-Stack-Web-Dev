using System.Collections.Generic;


namespace Fall2025_Project3_krhanson3.Models.ViewModels
{
    public class ActorViewModel
    {
        public int ActorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public int Age { get; set; }
        public string? IMDBUrl { get; set; }
        public byte[]? Photo { get; set; }

        // Movies the actor has been in
        public List<MovieInfo> Movies { get; set; } = new();

        public double? SentimentAverage { get; set; }

        public List<TweetsInfo> Tweets { get; set; } = new();

    }

    public class MovieInfo
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class TweetsInfo
    {
        public int Id { get; set; }
        public int ActorID { get; set; }
        public Actors Actor { get; set; }
        public string User { get; set; }
        public string Text { get; set; } = string.Empty;
        public double Sentiment { get; set; }
    }
}
