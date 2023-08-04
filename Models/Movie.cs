using System.ComponentModel.DataAnnotations.Schema;

namespace dvd_rental_app.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        public string? Title { get; set; }

        public int? Budget { get; set; }

        public string? Homepage { get; set; }

        public string? Overview { get; set; }

        public decimal? Popularity { get; set; }

        public string? Cast { get; set; }

        public string? Genre { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public long? Revenue { get; set; }

        public int? Runtime { get; set; }

        public string? MovieStatus { get; set; }

        public string? Tagline { get; set; }

        public decimal? VoteAverage { get; set; }

        public int? VoteCount { get; set; }
    }
}
