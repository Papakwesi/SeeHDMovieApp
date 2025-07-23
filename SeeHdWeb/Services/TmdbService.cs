using Microsoft.Extensions.Configuration;
using SeeHdWeb.Models.TMDB; 
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class TmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public TmdbService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["TMDB:ApiKey"];
        _baseUrl = config["TMDB:BaseUrl"];
    }

    public async Task<MovieSearchResult> SearchMoviesAsync(string query)
    {
        var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}";
        return await _httpClient.GetFromJsonAsync<MovieSearchResult>(url);
    }

    public async Task<MovieDetail> GetMovieDetailAsync(int movieId)
    {
        var url = $"{_baseUrl}/movie/{movieId}?api_key={_apiKey}";
        return await _httpClient.GetFromJsonAsync<MovieDetail>(url);
    }
    public async Task<string> GetTrailerKeyAsync(int movieId)
    {
        var url = $"{_baseUrl}/movie/{movieId}/videos?api_key={_apiKey}";
        var videoResult = await _httpClient.GetFromJsonAsync<VideoResult>(url);

        var trailer = videoResult?.results?
            .FirstOrDefault(v => v.site == "YouTube" && v.type == "Trailer");

        return trailer?.key;
    }

}
