using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;

namespace InfoTrackDevelopmentProject.Business.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;
        private const int MaxRetries = 3;
        private const int DelayMilliseconds = 2000; // 2 seconds

        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                    var response = await _httpClient.GetStringAsync(url);
                    var positions = FindUrlPositions(response, request.Url);

                    if(positions.Count == 0)
                    {
                        result.Positions.Add(-1); // Use -1 to indicate no valid URL found
                    }
                    else
                    {
                        result.Positions.AddRange(positions);
                    }
                    break; // Exit loop on success
                }
                catch(HttpRequestException ex) when(ex.Message.Contains("429"))
                {
                    Console.WriteLine("Rate limit exceeded. Retrying...");
                    retries++;
                    if(retries >= MaxRetries)
                    {
                        result.Positions.Add(-1); // Use -1 to indicate an error after retries
                    }
                    else
                    {
                        await Task.Delay(DelayMilliseconds * (int)Math.Pow(2, retries)); // Exponential backoff
                    }
                }
                catch(HttpRequestException ex)
                {
                    Console.WriteLine($"Error occurred while fetching search results: {ex.Message}");
                    result.Positions.Add(-1); // Use -1 to indicate an error
                    break; // Exit loop on other errors
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