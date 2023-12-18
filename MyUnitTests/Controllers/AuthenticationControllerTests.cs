using API.Controllers;
using API.Interfaces;
using API.Models;
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
    public class AuthenticationControllerTests
    {
        private Mock<UserManager<IdentityUser>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<SignInManager<IdentityUser>> _signInManager;
        private Mock<IMapper> _mapper;
        private Mock<IJWTManager> _jwtManager;
        private AuthenticationController _controller;

        [SetUp]
        public void Setup()
        {
            _jwtManager = new Mock<IJWTManager>();

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

            _mapper = new Mock<IMapper>();
            _controller = new AuthenticationController(_signInManager.Object ,
                _userManager.Object, _mapper.Object ,_roleManager.Object, _jwtManager.Object);
        }


        [Test]
        public async Task Register_UserCreationFails_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRegistrationModel();
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var res = await _controller.Register(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task Register_RoleAddingFails_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRegistrationModel(){ 
                Role = "Admin"};
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);
            _roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(new IdentityRole(model.Role));
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var res = await _controller.Register(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task Register_UserIsRegistered_ReturnsOk()
        {
            // Arrange
            var model = new UserRegistrationModel(){ Role = "Admin"};
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);
            _roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(new IdentityRole(model.Role));
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var res = await _controller.Register(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public async Task Login_LoginSuccessfull_ReturnsOk()
        {
            // Arrange
            var model = new UserLoginModel { Email = "email@email.com", Password = "pass" };
            var user = new IdentityUser { UserName = "email@email.com", Id = "testUserId" };
            _userManager.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _signInManager.Setup(x => x.PasswordSignInAsync(user.UserName, model.Password, true, false))
                          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _jwtManager.Setup(x => x.GenerateToken(user)).ReturnsAsync("token");

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult?.Value);
        }

        [Test]
        public async Task Login_LoginFails_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserLoginModel { Email = "email@email.com", Password = "password" };
            var user = new IdentityUser { UserName = "email@email.com" };
            _userManager.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _signInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password, true, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var res = await _controller.Login(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public async Task Logout_SignOutUser_ReturnsOkResult()
        {
            // Act
            var res = await _controller.LogOut();

            // Assert
            _signInManager.Verify(s => s.SignOutAsync());
            Assert.IsInstanceOf<OkObjectResult>(res);
        }
    }
}
