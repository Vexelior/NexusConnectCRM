using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NexusConnectCRM.Controllers;
using Xunit;

namespace NexusConnectCRM.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _logger;

        public HomeControllerTests()
        {
            _logger = new Mock<ILogger<HomeController>>();
        }

        [Fact]
        public async void Index_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(_logger.Object);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}