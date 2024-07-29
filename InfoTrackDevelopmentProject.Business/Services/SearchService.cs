using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackDevelopmentProject.Business.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;

        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SearchResult> GetSearchResultAsync(SearchRequest request)
        {
            var result = new SearchResult();
            var url = $"https://www.google.co.uk/search?num=100&q={Uri.EscapeDataString(request.Keywords)}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var positions = FindUrlPositions(response, request.Url);
                result.Positions.AddRange(positions);
            }
            catch(HttpRequestException ex)
            {
                // Handle exception or log error
                Console.WriteLine($"Error occurred while fetching search results: {ex.Message}");
            }

            return result;
        }

        private List<int> FindUrlPositions(string htmlContent, string url)
        {
            var positions = new List<int>();
            var lowerCaseHtmlContent = htmlContent.ToLower();
            var lowerCaseUrl = url.ToLower();
            int position = 0;
            int index = 0;

            while((index = lowerCaseHtmlContent.IndexOf(lowerCaseUrl, index)) != -1)
            {
                positions.Add(position + 1);
                index += lowerCaseUrl.Length;
                position++;
            }

            return positions;
        }
    }
}