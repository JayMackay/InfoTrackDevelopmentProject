using InfoTrackDevelopmentProject.Business.Services;
using InfoTrackDevelopmentProject.Domain.Entities;
using Moq;
using Moq.Protected;
using System.Net;


namespace InfoTrackDevelopmentProject.Tests.ServiceTests
{
    [TestFixture]
    internal class SearchServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private SearchService _searchService;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            // Create HttpClient with mocked handler
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Initialize SearchService
            _searchService = new SearchService(httpClient);
        }

        [Test]
        public async Task GetSearchResultAsync_ReturnsPositions_WhenUrlIsFound()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = "https://www.example.com" };
            var htmlContent = "<html><body><a href=\"https://www.example.com\">Example</a></body></html>";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(htmlContent)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req != null && req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("example")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _searchService.GetSearchResultAsync(searchRequest);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(result.Positions); // Ensure the Positions list is not null
            Assert.That(result.Positions, Is.Not.Empty);
            Assert.That(result.Positions, Does.Contain(1));
        }

        [Test]
        public async Task GetSearchResultAsync_ReturnsEmptyPositions_WhenUrlIsNotFound()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = "https://www.notfound.com" };
            var htmlContent = "<html><body><a href=\"https://www.example.com\">Example</a></body></html>";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(htmlContent)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req != null && req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("example")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _searchService.GetSearchResultAsync(searchRequest);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(result.Positions); // Ensure the Positions list is not null
            Assert.IsEmpty(result.Positions);
        }

        [Test]
        public async Task GetSearchResultAsync_HandlesError_WhenHttpRequestExceptionIsThrown()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = "https://www.example.com" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req != null && req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("example")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _searchService.GetSearchResultAsync(searchRequest);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.NotNull(result.Positions); // Ensure the Positions list is not null
            Assert.IsEmpty(result.Positions);
        }
    }
}