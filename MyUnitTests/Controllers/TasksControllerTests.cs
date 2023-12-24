using API.Controllers;
using Services.Abstractions.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<ITasksService> _taskService;
        private Mock<IMapper> _mapper;
        private Mock<UserManager<IdentityUser>> _userManager;
        private TasksController _controller;

        [SetUp]
        public void Setup()
        {
            _taskService = new Mock<ITasksService>();
            _mapper = new Mock<IMapper>();
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

            _controller = new TasksController(_taskService.Object, _mapper.Object, _userManager.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "testUserId"),
            }));
            _controller.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        [TestCase("testUserId")]
        public async Task AddTask_BaseTaskAdded_TaskAdded(string userId)
        {
            // Arrange
            var inputTask = new TaskInputModel(){
                TaskType = Domain.Entities.TaskType.Task,
                BaseTaskId = null,
                Category = Domain.Entities.TaskCategory.Work,
                Status = Domain.Entities.TaskStatus.None,
                Name = "TaskName",
                Description = "TaskDescription",
                DueDate = DateTime.Today.AddDays(1),
                UserId = userId
            };

            var task = new Tasks(){
                TaskType = TaskType.Task,
                BaseTaskId = null,
                Category = TaskCategory.Work,
                Status = Domain.Entities.TaskStatus.None,
                Name = "TaskName",
                Description = "TaskDescription",
                DueDate = DateTime.Today.AddDays(1),
                UserId = userId
            };

            _mapper.Setup(m => m.Map<Tasks>(It.IsAny<TaskInputModel>())).Returns(task);

            // Act
            
            var res = await _controller.AddTask(userId, inputTask);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(res);
            var createdAtResult = res as CreatedAtActionResult;
            Assert.AreEqual(nameof(TasksController.GetTaskById), createdAtResult.ActionName);
            Assert.AreEqual(task, createdAtResult.Value);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task AddTask_UserDoesNotHaveAccess_ReturnsForbid(string userId)
        {
            // Arrange
            var inputTask = new TaskInputModel();

            // Act
            var res = await _controller.AddTask(userId, inputTask);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_UserHasAccess_ReturnsOk(string userId)
        {
            // Arrange
            var taskList = new List<Tasks>();
            _taskService.Setup(x => x.GetTasksForUser(It.IsAny<TasksByTypeAndStatusAndCategorySpecAndUserId>()))
                .ReturnsAsync(taskList);

            // Act
            var result = await _controller.GetTasksWithFilter(userId, Domain.Entities.TaskType.Task, Domain.Entities.TaskStatus.Done, Domain.Entities.TaskCategory.Fitness);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.AreEqual(taskList, okResult?.Value);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task GetTasksWithFilter_UserDoesNotHaveAccess_ReturnsForbid(string userId)
        {
            // Act
            var result = await _controller.GetTasksWithFilter(userId, Domain.Entities.TaskType.Task, null, null);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task GetTaskById_UserDoesNotHaveAccess_ReturnsForbid(string userId)
        {
            // Arrange
            int taskId = 1;

            // Act
            var res = await _controller.GetTaskById(userId, taskId);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTaskById_TaskDoesNotExist_ReturnsNotFound(string userId)
        {
            // Arrange
            int taskId = 1;
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync((Tasks)null);

            // Act
            var res = await _controller.GetTaskById(userId, taskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTaskById_TaskExists_ReturnsOk(string userId)
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks();
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync(task);

            // Act
            var res = await _controller.GetTaskById(userId, taskId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(res);
            var okResult = res as OkObjectResult;
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.AreEqual(task, okResult?.Value);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task UpdateTask_UserDoesNotHaveAccess_ReturnsForbid(string userId)
        {
            // Arrange
            int taskId = 1;
            var model = new TaskInputModel();

            // Act
            var res = await _controller.UpdateTask(userId, taskId, model);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task UpdateTask_TaskDoesNotExists_ReturnsNotFound(string userId)
        {
            // Arrange
            int taskId = 1;
            var model = new TaskInputModel();
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync((Tasks)null);

            // Act
            var res = await _controller.UpdateTask(userId, taskId, model);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task UpdateTask_TaskIsUpdated_ReturnsNoContent(string userId)
        {
            // Arrange
            int taskId = 1;
            var model = new TaskInputModel();
            var task = new Tasks();
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync(task);

            // Act
            var res = await _controller.UpdateTask(userId, taskId, model);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(res);
        }

        [Test]
        [TestCase("testUserId")] 
        public async Task Delete_TaskDoesNotExists_ReturnsNotFound(string userId)
        {
            // Arrange
            int taskId = 1;
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync((Tasks)null);

            // Act
            var res = await _controller.Delete(userId, taskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task Delete_TaskIsDeleted_ReturnsNoContent(string userId)
        {
            // Arrange
            int taskId = 1;
            var task = new Tasks();
            _taskService.Setup(x => x.GetTaskById(userId, taskId)).ReturnsAsync(task);

            // Act
            var res = await _controller.Delete(userId, taskId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(res);
        }
    }
}
