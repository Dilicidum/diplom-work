using AutoFixture.NUnit3;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Service.Services;
using Services.Abstractions.Interfaces;
using Moq;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TasksServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IVacancyRepository> _tasksRepository;
        private Mock<ITaskValidationService> _taskValidationService;
        private VacanciesService _service;

        [SetUp]
        public void Setup()
        {
            _tasksRepository = new Mock<IVacancyRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _taskValidationService= new Mock<ITaskValidationService>();
            _service = new VacanciesService(_unitOfWork.Object,_taskValidationService.Object);
            _unitOfWork.Setup(x => x.TasksRepository).Returns(_tasksRepository.Object);
        }

        [Test]
        public async Task AddTask_BaseTaskNotExists_TaskAdded()
        {
            //Arrange
            Vacancy task = new Vacancy();

            //Act
            await _service.AddVacancy(task);

            //Assert
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task AddTask_BaseTasksExists_TaskAdded()
        {
            // Arrange
            var task = new Vacancy { TaskType = TaskType.SubTask};
            _taskValidationService.Setup(x => x.ValidateTaskExistence(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            await _service.AddVacancy(task);

            // Assert
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task AddTask_BaseTasksExists_TaskNotAdded()
        {
            // Arrange
            var task = new Vacancy { TaskType = TaskType.SubTask};
            _taskValidationService.Setup(x => x.ValidateTaskExistence(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            await _service.AddVacancy(task);

            // Assert
            _unitOfWork.Verify(x => x.Save(), Times.Never);
        }

        [Test]
        public async Task DeleteTask_TaskHasSubTasks_TasksAndSubTasksDeleted()
        {
            // Arrange
            var taskId = 1;
            var subTasks = new List<Vacancy>
            {
                new Vacancy { Id = 2, TaskType = TaskType.SubTask },
            };
            var task = new Vacancy { Id = taskId, TaskType = TaskType.Task };

            //_unitOfWork.Setup(x => x.TasksRepository.ListAsync(It.IsAny<SubTasksByBaseTaskIdSpec>(),It.IsAny<CancellationToken>()))
              //         .ReturnsAsync(subTasks);

            // Act
            await _service.DeleteVacancy(task);

            // Assert
            foreach (var subTask in subTasks)
            {
                _unitOfWork.Verify(x => x.TasksRepository.DeleteAsync(subTask,It.IsAny<CancellationToken>()));
            }

            _unitOfWork.Verify(x => x.TasksRepository.DeleteAsync(task,It.IsAny<CancellationToken>()));
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task DeleteTask_TaskHasNoSubTasks_NoTasksDeleted()
        {
            // Arrange
            var task = new Vacancy { Id = 1, TaskType = TaskType.Task };
            //_unitOfWork.Setup(x => x.TasksRepository.ListAsync(It.IsAny<SubTasksByBaseTaskIdSpec>(), It.IsAny<CancellationToken>()))
              //         .ReturnsAsync(new List<Tasks>());

            // Act
            await _service.DeleteVacancy(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.DeleteAsync(task, It.IsAny<CancellationToken>()));
            _unitOfWork.Verify(x => x.Save());
        }


        [Test]
        public async Task UpdateTask_TaskAdded()
        {
            // Arrange
            var task = new Vacancy();

            // Act
            await _service.UpdateVacancy(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.UpdateAsync(task, It.IsAny<CancellationToken>()));
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        [TestCase("testUserId",5)]
        public async Task GetTaskById_TaskTypeSubTask_ReturnsOnlyTaskRequested(string userId, int taskId)
        {
            // Arrange
            var task = GetTasks().Where(x => x.Id == taskId).FirstOrDefault();

            //_unitOfWork.Setup(x => x.TasksRepository.FirstOrDefaultAsync(It.IsAny<TaskByUserIdAndTaskIdSpec>(), It.IsAny<CancellationToken>()))
              //     .ReturnsAsync(task);

            // Act
            var res = await _service.GetVacancyById(userId, taskId);

            // Assert
            Assert.AreEqual(taskId, res.Id);

        }

        private IEnumerable<Vacancy> GetTasks()
        {
            return new List<Vacancy>
            {
                new Vacancy { Id = 1, UserId = "testUserId",TaskType = TaskType.Task,Status = Domain.Entities.TaskStatus.Done, Category = TaskCategory.Sales},
                new Vacancy { Id = 2, UserId = "wrongUserId", TaskType = TaskType.Task, Status = Domain.Entities.TaskStatus.Done, Category = TaskCategory.Sales},
                new Vacancy { Id = 3, UserId = "testUserId", TaskType = TaskType.SubTask, Status = Domain.Entities.TaskStatus.Done, Category = TaskCategory.Sales},
                new Vacancy { Id = 4, UserId = "testUserId", TaskType = TaskType.Task, Status = Domain.Entities.TaskStatus.Rejected, Category = TaskCategory.Sales},
                new Vacancy { Id = 5, UserId = "testUserId", TaskType = TaskType.SubTask, Status = Domain.Entities.TaskStatus.Rejected, Category = TaskCategory.Sales},
                new Vacancy { Id = 6, UserId = "wrongUserId", TaskType = TaskType.SubTask, Status = Domain.Entities.TaskStatus.Rejected, Category = TaskCategory.Sales},
            };
        }
    }
}
