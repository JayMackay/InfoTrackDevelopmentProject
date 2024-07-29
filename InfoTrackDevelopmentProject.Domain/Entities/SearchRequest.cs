namespace InfoTrackDevelopmentProject.Domain.Entities
{
    public class SearchRequest
    {
        public string Keywords { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}