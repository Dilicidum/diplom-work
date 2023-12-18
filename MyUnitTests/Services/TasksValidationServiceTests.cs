using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TaskValidationServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private TaskValidationService _service; 

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new TaskValidationService(_unitOfWork.Object);
        }

        [Test]
        [TestCase(1)]
        public async Task ValidateTaskExistence_ShouldReturnTrue_WhenTaskExists(int taskId)
        {
            // Arrange
            var task = new Tasks { Id = taskId};

            _unitOfWork.Setup(x => x.TasksRepository.GetById(taskId)).ReturnsAsync(task);

            // Act
            var res = await _service.ValidateTaskExistence(taskId);

            // Assert
            Assert.IsTrue(res);
        }

        [Test]
        [TestCase(2)]
        public async Task ValidateTaskExistence_ShouldReturnFalse_WhenTaskDoesNotExist(int taskId)
        {
            // Arrange
            _unitOfWork.Setup(x => x.TasksRepository.GetById(taskId)).ReturnsAsync((Tasks)null);

            // Act
            var result = await _service.ValidateTaskExistence(taskId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
