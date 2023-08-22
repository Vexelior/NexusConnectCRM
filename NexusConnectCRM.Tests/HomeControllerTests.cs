using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NexusConnectCRM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(_logger.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void About_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(_logger.Object);

            // Act
            var result = controller.About();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(_logger.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void FAQ_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(_logger.Object);

            // Act
            var result = controller.FAQ();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
