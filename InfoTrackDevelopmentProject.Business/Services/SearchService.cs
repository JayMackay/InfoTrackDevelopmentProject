using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace InfoTrackDevelopmentProject.Business.Services
{
    public class SearchService : ISearchService
    {
        private const int MaxRetries = 3;
        private const int DelayMilliseconds = 2000; // 2 seconds
        private readonly ILogger<SearchService> _logger;

        public SearchService(ILogger<SearchService> logger)
        {
            _logger = logger;
        }

        public async Task<SearchResult> GetSearchResultAsync(SearchRequest request)
        {
            var result = new SearchResult();
            var url = $"https://www.google.co.uk/search?num=100&q={Uri.EscapeDataString(request.Keywords)}";

            int retries = 0;

            while(retries < MaxRetries)
            {
                try
                {
                    // Create a new httpClient to refresh the service for consecutive runs
                    using var httpClient = new HttpClient();

                    var response = await httpClient.GetStringAsync(url);

                    var responseContent = response.Length > 500 ? response[..500] : response;
                    _logger.LogInformation("Response Content: {ResponseContent}", responseContent);

                    var positions = FindUrlPositions(response, request.Url);

                    if(positions.Count == 0)
                    {
                        result.Positions.Add(-1);
                    }
                    else
                    {
                        result.Positions.AddRange(positions);
                    }
                    break;
                }
                catch(HttpRequestException ex) when(ex.Message.Contains("429"))
                {
                    _logger.LogWarning(ex, "Rate limit exceeded. Retrying...");
                    retries++;
                    if(retries >= MaxRetries)
                    {
                        result.Positions.Add(-1);
                    }
                    else
                    {
                        await Task.Delay(DelayMilliseconds * (int)Math.Pow(2, retries));
                    }
                }
                catch(HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error occurred while fetching search results.");
                    result.Positions.Add(-1);
                    break;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error.");
                    result.Positions.Add(-1);
                    break;
                }
            }

            return result;
        }

        private static List<int> FindUrlPositions(string htmlContent, string url)
        {
            var positions = new List<int>();
            var lowerCaseHtmlContent = htmlContent.ToLower();
            var lowerCaseUrl = url.ToLower();
            int position = 1; // Start positions from 1
            int index = 0;

            while((index = lowerCaseHtmlContent.IndexOf(lowerCaseUrl, index)) != -1)
            {
                positions.Add(position);
                index += lowerCaseUrl.Length; // Move past the current match
                position++; // Increment position counter
            }

            return positions;
        }
    }
}
