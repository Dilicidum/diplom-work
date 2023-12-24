using API.Controllers;
using Services.Abstractions.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<UserManager<IdentityUser>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<SignInManager<IdentityUser>> _signInManager;
        private Mock<IMapper> _mapperMock;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
                _roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

                _userManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object,
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object);

                _signInManager = new Mock<SignInManager<IdentityUser>>(
                _userManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
                null,
                null,
                null,
                null);
            _mapperMock = new Mock<IMapper>();

            _controller = new UsersController(_userManager.Object, _mapperMock.Object, _roleManager.Object);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetUserById_UserExists_ReturnsOkObjectResult(string userId)
        {
            // Arrange
            var mockUser = new IdentityUser { Id = userId };
            _userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(mockUser);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task GetUserById_UserDoesNotExist_ReturnsNotFoundResult(string userId)
        {
            // Arrange
            _userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task CreateUser_UserCreationSucceded_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var userModel = new UserRegistrationModel {};
            var identityUser = new IdentityUser {};
            _mapperMock.Setup(m => m.Map<IdentityUser>(It.IsAny<UserRegistrationModel>())).Returns(identityUser);
            _userManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _roleManager.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole(userModel.Role));
            _userManager.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.CreateUser(userModel);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public async Task CreateUser_UserCreationFailed_ReturnsBadRequestResult()
        {
            // Arrange
            var userModel = new UserRegistrationModel {};
            _mapperMock.Setup(m => m.Map<IdentityUser>(It.IsAny<UserRegistrationModel>())).Returns(new IdentityUser());
            _userManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.CreateUser(userModel);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
