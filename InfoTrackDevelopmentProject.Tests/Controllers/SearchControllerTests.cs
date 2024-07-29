using InfoTrackDevelopmentProject.Controllers;
using InfoTrackDevelopmentProject.Domain.Entities;
using InfoTrackDevelopmentProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InfoTrackDevelopmentProject.Tests.ControllerTests
{
    [TestFixture]
    public class SearchControllerTests
    {
        private Mock<ISearchService> _searchServiceMock;
        private SearchController _controller;

        [SetUp]
        public void Setup()
        {
            _searchServiceMock = new Mock<ISearchService>();
            _controller = new SearchController(_searchServiceMock.Object);
        }

        [Test]
        public async Task Post_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.Post(null!);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var actionResult = result as BadRequestObjectResult;
            Assert.NotNull(actionResult, "Action result should not be null.");
            Assert.That(actionResult.StatusCode, Is.EqualTo(400));
            Assert.That(actionResult.Value, Is.EqualTo("Search request cannot be null."));
        }

        [Test]
        public async Task Post_ReturnsBadRequest_WhenKeywordsAreMissing()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = string.Empty, Url = "https://www.example.com" };

            // Act
            var result = await _controller.Post(searchRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var actionResult = result as BadRequestObjectResult;
            Assert.NotNull(actionResult, "Action result should not be null.");
            Assert.That(actionResult.StatusCode, Is.EqualTo(400));
            Assert.That(actionResult.Value, Is.EqualTo("Keywords cannot be empty."));
        }

        [Test]
        public async Task Post_ReturnsBadRequest_WhenUrlIsMissing()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = string.Empty };

            // Act
            var result = await _controller.Post(searchRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var actionResult = result as BadRequestObjectResult;
            Assert.NotNull(actionResult, "Action result should not be null.");
            Assert.That(actionResult.StatusCode, Is.EqualTo(400));
            Assert.That(actionResult.Value, Is.EqualTo("URL cannot be empty."));
        }

        [Test]
        public async Task Post_ReturnsOk_WhenRequestIsValid()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = "https://www.example.com" };
            var searchResult = new SearchResult { Positions = new List<int> { 1 } };

            _searchServiceMock.Setup(service => service.GetSearchResultAsync(searchRequest))
                .ReturnsAsync(searchResult);

            // Act
            var result = await _controller.Post(searchRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var actionResult = result as OkObjectResult;
            Assert.NotNull(actionResult, "Action result should not be null.");
            Assert.That(actionResult.StatusCode, Is.EqualTo(200));
            Assert.That(actionResult.Value, Is.EqualTo(searchResult));
        }

        [Test]
        public async Task Post_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var searchRequest = new SearchRequest { Keywords = "example", Url = "https://www.example.com" };

            _searchServiceMock.Setup(service => service.GetSearchResultAsync(searchRequest))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Post(searchRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var actionResult = result as ObjectResult;
            Assert.NotNull(actionResult, "Action result should not be null.");
            Assert.That(actionResult.StatusCode, Is.EqualTo(500));
            Assert.That(actionResult.Value, Is.EqualTo("An error occurred while processing the request."));
        }
    }
}