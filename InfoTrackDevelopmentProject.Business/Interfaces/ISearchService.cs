using InfoTrackDevelopmentProject.Domain.Entities;

namespace InfoTrackDevelopmentProject.Business.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResult> GetSearchResultAsync(SearchRequest request);
    }
}