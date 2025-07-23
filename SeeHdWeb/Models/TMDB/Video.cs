using System.Collections.Generic;

namespace SeeHdWeb.Models.TMDB
{
    public class VideoResult
    {
        public List<Video> results { get; set; }
    }

    public class Video
    {
        public string key { get; set; }      // YouTube video key
        public string name { get; set; }
        public string site { get; set; }
        public string type { get; set; }
    }
}
