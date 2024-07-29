using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackDevelopmentProject.Business.Services
{
    public class SearchService : ISearchService
    {
        private const int MaxRetries = 3;
        private const int DelayMilliseconds = 2000; // 2 seconds
        private readonly ILogger<SearchService> _logger;
        private readonly IConfiguration _configuration;

        public SearchService(ILogger<SearchService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<SearchResult> GetSearchResultAsync(SearchRequest request)
        {
            var result = new SearchResult();
            var url = GetSearchUrl(request.Keywords, request.SearchEngine);

            int retries = 0;

            while(retries < MaxRetries)
            {
                try
                {
                    // Create a new HttpClient to refresh the service for consecutive runs
                    using var httpClient = new HttpClient();

                    var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                    var responseCode = (int)response.StatusCode;
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var truncatedContent = responseContent.Length > 500 ? responseContent[..500] : responseContent;
                    _logger.LogInformation("Received response from {Url}. Status Code: {ResponseCode}. Response length: {ResponseLength} characters. Truncated content: {TruncatedContent}",
                        url, responseCode, responseContent.Length, truncatedContent);

                    if(response.IsSuccessStatusCode)
                    {
                        var positions = FindUrlPositions(responseContent, request.Url);

                        if(positions.Count == 0)
                        {
                            _logger.LogInformation("No valid URL found with the matching phrase: {SearchPhrase}", request.Url);
                            result.Positions.Add(-1);
                        }
                        else
                        {
                            result.Positions.AddRange(positions);
                        }
                        break;
                    }
                    else if(responseCode == 429) // Rate limit exceeded
                    {
                        _logger.LogWarning("Search engine rate limit exceeded. Retrying...");
                        retries++;
                        if(retries >= MaxRetries)
                        {
                            _logger.LogWarning("Max retries reached. No valid URL found with the matching phrase: {SearchPhrase}", request.Url);
                            result.Positions.Add(-1);
                        }
                        else
                        {
                            await Task.Delay(DelayMilliseconds * (int)Math.Pow(2, retries));
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Received non-success status code: {ResponseCode}. Retrying...", responseCode);
                        retries++;
                        if(retries >= MaxRetries)
                        {
                            _logger.LogWarning("Max retries reached. No valid URL found with the matching phrase: {SearchPhrase}", request.Url);
                            result.Positions.Add(-1);
                        }
                        else
                        {
                            await Task.Delay(DelayMilliseconds * (int)Math.Pow(2, retries));
                        }
                    }
                }
                catch(HttpRequestException ex) when(ex.Message.Contains("429"))
                {
                    _logger.LogWarning(ex, "Rate limit exceeded. Retrying...");
                    retries++;
                    if(retries >= MaxRetries)
                    {
                        _logger.LogWarning("Max retries reached. No valid URL found with the matching phrase: {SearchPhrase}", request.Url);
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

        private string GetSearchUrl(string keywords, string searchEngine)
        {
            var baseUrl = searchEngine.ToLower() switch
            {
                "bing" => _configuration["SearchEngines:Bing"],
                "yahoo" => _configuration["SearchEngines:Yahoo"],
                _ => _configuration["SearchEngines:Google"],
            };
            return $"{baseUrl}{Uri.EscapeDataString(keywords)}";
        }
    }
}
