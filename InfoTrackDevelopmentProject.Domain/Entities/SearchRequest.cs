namespace InfoTrackDevelopmentProject.Domain.Entities
{
    public class SearchRequest
    {
        public required string Keywords { get; set; }
        public required string Url { get; set; }
        public string SearchEngine { get; set; } = "google"; // Default to Google
    }
}