namespace SeeHdWeb.Models.TMDB
{
    public class MovieSearchResult
    {
        public int page { get; set; }
        public List<TmdbMovie> results { get; set; }
    }

    public class TmdbMovie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public double vote_average { get; set; }
        public string release_date { get; set; }
    }
}
