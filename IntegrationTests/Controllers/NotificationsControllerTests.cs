using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using DAL;
using DAL.Models;
using System.Text.Json;
using IntegrationTests.Repositories;
using IntegrationTests.Helpers;
using BLL.Models;

namespace IntegrationTests.Controllers
{
    [TestFixture]
    public class NotificationsControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();

                Utilities.InitializeDbForTests(db);
            }
        }

        [Test]
        public async Task GetNotifications_NotificationsReturns_ReturnsOk()
        {
            //Act
            var response = await _client.GetAsync("users/testUserId/notifications");

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetNotifications_UserDoesNotHaveAccess_ReturnsForbidden()
        {
            //Act
            var response = await _client.GetAsync("users/wrongUserId/notifications");

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        public async Task GetNotifications_UserHasAccessAndSomeNotifications_ReturnsNotifications()
        {
            //Act
            var response = await _client.GetAsync("users/testUserId/notifications");
            var result = response.Content.ReadAsStringAsync();
            
            //Assert
            var notifications = JsonSerializer.Deserialize<List<Notification>>(result.Result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, notifications?.Count());
        }
    }
}
