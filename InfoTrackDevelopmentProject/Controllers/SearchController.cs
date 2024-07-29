using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackDevelopmentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchRequest request)
        {
            if(request == null)
            {
                _logger.LogWarning("Search request is null.");
                return BadRequest("Search request cannot be null.");
            }

            if(string.IsNullOrWhiteSpace(request.Keywords))
            {
                _logger.LogWarning("Search request keywords are missing.");
                return BadRequest("Keywords cannot be empty.");
            }

            if(string.IsNullOrWhiteSpace(request.Url))
            {
                _logger.LogWarning("Search request URL is missing.");
                return BadRequest("URL cannot be empty.");
            }

            try
            {
                var result = await _searchService.GetSearchResultAsync(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the search request.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}