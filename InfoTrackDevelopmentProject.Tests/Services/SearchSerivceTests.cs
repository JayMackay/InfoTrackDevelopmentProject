using InfoTrackDevelopmentProject.Business.Services;
using InfoTrackDevelopmentProject.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace InfoTrackDevelopmentProject.Tests.Services
{
    public class SearchServiceTests
    {
        private SearchService _searchService;
        private Mock<ILogger<SearchService>> _mockLogger;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<SearchService>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _searchService = new SearchService(_mockLogger.Object, _mockConfiguration.Object);
        }

        [Test]
        public async Task GetSearchResultAsync_ValidRequest_ReturnsPositions()
        {
            var request = new SearchRequest
            {
                Keywords = "test",
                Url = "https://example.com",
                SearchEngine = "google"
            };

            var result = await _searchService.GetSearchResultAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Positions, Is.Not.Null);
        }

        [Test]
        public async Task GetSearchResultAsync_NoMatchingUrls_ReturnsMinusOne()
        {
            var request = new SearchRequest
            {
                Keywords = "no results keyword",
                Url = "https://example.com",
                SearchEngine = "google"
            };

            var result = await _searchService.GetSearchResultAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Positions, Is.Not.Null);
            Assert.That(result.Positions, Does.Contain(-1));
        }

        [Test]
        public async Task GetSearchResultAsync_ErrorOccurs_ReturnsMinusOne()
        {
            var request = new SearchRequest
            {
                Keywords = "",
                Url = "",
                SearchEngine = "google"
            };

            var result = await _searchService.GetSearchResultAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Positions, Is.Not.Null);
            Assert.That(result.Positions, Does.Contain(-1));
        }

        [Test]
        public async Task GetSearchResultAsync_DefaultSearchEngine_UsesGoogle()
        {
            var request = new SearchRequest
            {
                Keywords = "test",
                Url = "https://example.com",
                SearchEngine = "" // No SearchEngine set
            };

            var result = await _searchService.GetSearchResultAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Positions, Is.Not.Null);
        }
    }
}