using API.Controllers;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class NotificationsControllerTests
    {
        private Mock<INotificationsService> _notificationService;
        private NotificationsController _controller;


        [SetUp]
        public void Setup()
        {   
            _notificationService = new Mock<INotificationsService>();
            _controller = new NotificationsController(_notificationService.Object);

        
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "testUserId"),
            }));
            _controller.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetNotifications_UserAuthorized_GetsNotifications(string userId)
        {
            // Arrange
            var notifications = GetNotifications();
            _notificationService.Setup(s => s.GetNotifications(userId)).ReturnsAsync(notifications);

            // Act
            var res = await _controller.GetNotifications(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
            
            var okResult = res as OkObjectResult;
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.AreEqual(notifications, okResult?.Value);
        }

        [Test]
        [TestCase("wrongUserId")] 
        public async Task GetNotifications_UserNotAuthorized_ReturnsForbidden(string userId)
        {
            // Act
            var res = await _controller.GetNotifications(userId);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(res);
        }

        private IEnumerable<Notification> GetNotifications()
        {
            return new List<Notification>()
            {
                new Notification() {TaskId = 1, Title = "Title1", UserId = "A"},
                new Notification() {TaskId = 2, Title = "Titile2", UserId = "A"}
            };
        }
    }
}
