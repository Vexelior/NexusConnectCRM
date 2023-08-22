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
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        } 
    }
}
