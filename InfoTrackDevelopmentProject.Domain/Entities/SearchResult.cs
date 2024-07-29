namespace InfoTrackDevelopmentProject.Domain.Entities
{
    public class SearchResult
    {
        public List<int> Positions { get; set; }
        public SearchResult()
        {
            Positions = new List<int>();
        }
    }
}