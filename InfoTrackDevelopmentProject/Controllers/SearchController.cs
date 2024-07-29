using InfoTrackDevelopmentProject.Business.Interfaces;
using InfoTrackDevelopmentProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InfoTrackDevelopmentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchRequest request)
        {
            if(request == null)
            {
                Console.WriteLine("Search request is null.");
                return BadRequest("Search request cannot be null.");
            }

            if(string.IsNullOrWhiteSpace(request.Keywords))
            {
                Console.WriteLine("Search request keywords are missing.");
                return BadRequest("Keywords cannot be empty.");
            }

            if(string.IsNullOrWhiteSpace(request.Url))
            {
                Console.WriteLine("Search request URL is missing.");
                return BadRequest("URL cannot be empty.");
            }

            try
            {
                var result = await _searchService.GetSearchResultAsync(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the search request: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}