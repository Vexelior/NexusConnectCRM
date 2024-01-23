using NexusConnectCRM.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexusConnectCRM.Controllers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.ViewModels;

namespace NexusConnectCRM.Tests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _controller;
        private ContactController _contactController;
        private Mock<ILogger<ContactController>>? _contactMockRepo;
        private Mock<ILogger<HomeController>>? _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ILogger<HomeController>>();
            _contactMockRepo = new Mock<ILogger<ContactController>>();
            _controller = new HomeController(_mockRepo.Object);
            _contactController = new ContactController(_contactMockRepo.Object);
        }

        [Test]
        public async Task Index_ReturnsAViewResult()
        {
            var result = await _controller.Index();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Newsletter_InvalidModelState_ReturnsView()
        {
            _contactController.ModelState.AddModelError("Email", "Email is required");

            var newsletter = new NewsletterViewModel
            {
                Email = ""
            };

            var result = await _contactController.NewsletterSignUp(newsletter);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }
}
