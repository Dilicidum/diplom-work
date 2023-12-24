using AutoFixture.NUnit3;
using AutoMapper;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Specifications;
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
        private Mock<ITasksRepository> _tasksRepository;
        private Mock<ITaskValidationService> _taskValidationService;
        private TasksService _service;

        [SetUp]
        public void Setup()
        {
            _tasksRepository = new Mock<ITasksRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _taskValidationService= new Mock<ITaskValidationService>();
            _service = new TasksService(_unitOfWork.Object,_taskValidationService.Object);
            _unitOfWork.Setup(x => x.TasksRepository).Returns(_tasksRepository.Object);
        }

        [Test]
        public async Task AddTask_BaseTaskNotExists_TaskAdded()
        {
            //Arrange
            Tasks task = new Tasks();

            //Act
            await _service.AddTask(task);

            //Assert
            _unitOfWork.Verify(x => x.TasksRepository.Add(task));
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task AddTask_BaseTasksExists_TaskAdded()
        {
            // Arrange
            var task = new Tasks { TaskType = TaskType.SubTask, BaseTaskId = 1 };
            _taskValidationService.Setup(x => x.ValidateTaskExistence(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            await _service.AddTask(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.Add(task));
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task AddTask_BaseTasksExists_TaskNotAdded()
        {
            // Arrange
            var task = new Tasks { TaskType = TaskType.SubTask, BaseTaskId = 1 };
            _taskValidationService.Setup(x => x.ValidateTaskExistence(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            await _service.AddTask(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.Add(It.IsAny<Tasks>()), Times.Never);
            _unitOfWork.Verify(x => x.Save(), Times.Never);
        }

        [Test]
        public async Task DeleteTask_TaskHasSubTasks_TasksAndSubTasksDeleted()
        {
            // Arrange
            var taskId = 1;
            var subTasks = new List<Tasks>
            {
                new Tasks { Id = 2, BaseTaskId = taskId, TaskType = TaskType.SubTask },
            };
            var task = new Tasks { Id = taskId, TaskType = TaskType.Task };

            _unitOfWork.Setup(x => x.TasksRepository.Get(It.IsAny<Expression<Func<Tasks, bool>>>()))
                       .ReturnsAsync(subTasks);

            // Act
            await _service.DeleteTask(task);

            // Assert
            foreach (var subTask in subTasks)
            {
                _unitOfWork.Verify(x => x.TasksRepository.Delete(subTask));
            }

            _unitOfWork.Verify(x => x.TasksRepository.Delete(task));
            _unitOfWork.Verify(x => x.Save());
        }

        [Test]
        public async Task DeleteTask_TaskHasNoSubTasks_NoTasksDeleted()
        {
            // Arrange
            var task = new Tasks { Id = 1, TaskType = TaskType.Task };
            _unitOfWork.Setup(x => x.TasksRepository.Get(It.IsAny<Expression<Func<Tasks, bool>>>()))
                       .ReturnsAsync(new List<Tasks>());

            // Act
            await _service.DeleteTask(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.Delete(task));
            _unitOfWork.Verify(x => x.Save());
        }


        [Test]
        public async Task UpdateTask_TaskAdded()
        {
            // Arrange
            var task = new Tasks();

            // Act
            await _service.UpdateTask(task);

            // Assert
            _unitOfWork.Verify(x => x.TasksRepository.Update(task));
            _unitOfWork.Verify(x => x.Save());
        }

        [TestCase("testUserId")]
        [Test]
        public async Task GetTasksForUser_TaskTypeTaskNoOtherSpecifications_ReturnsAllBaseTasksForUser
            (string userId,TaskType taskType = TaskType.Task)
        {
            // Arrange
            var tasks = GetTasks();
            TaskTypeSpecification specification = new TaskTypeSpecification(taskType);
            _unitOfWork.Setup(x => x.TasksRepository.Find(specification))
                           .ReturnsAsync(tasks.Where(x => x.TaskType == taskType));

            // Act
            var res = await _service.GetTasksForUser(userId, specification);

            // Assert
            Assert.AreEqual(res.Count(),2);
        }

        [TestCase("testUserId")]
        [Test]
        public async Task GetTasksForUser_TaskTypeTaskAndTaskStatusDone_ReturnsAllBaseTasksForUserWithTaskStatusDone
            (string userId,TaskType taskType = TaskType.Task,DAL.Models.TaskStatus status = DAL.Models.TaskStatus.Done)
        {
            // Arrange
            var tasks = GetTasks();            
            Specification<Tasks> specification = new TaskTypeSpecification(taskType);
            specification =  specification.AndSpecification(new StatusSpecification(status));
            _unitOfWork.Setup(x => x.TasksRepository.Find(specification))
                           .ReturnsAsync(tasks.Where(x => x.TaskType == taskType && x.Status == status));

            // Act
            var res = await _service.GetTasksForUser(userId, specification);

            // Assert
            Assert.AreEqual(res.Count(),1);
        }

        [TestCase("testUserId")]
        [Test]
        public async Task GetTasksForUser_TaskTypeTaskAndTaskStatusDoneAndTaskCategoryWork_ReturnsAllBaseTasksForUserWithTaskStatusDoneAndCategoryWork
            (string userId,TaskType taskType = TaskType.Task,DAL.Models.TaskStatus status = DAL.Models.TaskStatus.Done,TaskCategory category = TaskCategory.Work)
        {
            // Arrange
            var tasks = GetTasks();
            Specification<Tasks> specification = new TaskTypeSpecification(taskType);
            specification =  specification.AndSpecification(new StatusSpecification(status));
            specification =  specification.AndSpecification(new TaskCategorySpecification(category));
            _unitOfWork.Setup(x => x.TasksRepository.Find(specification))
                           .ReturnsAsync(tasks.Where(x => x.TaskType == taskType && x.Status == status && x.Category == category));

            // Act
            var res = await _service.GetTasksForUser(userId, specification);

            // Assert
            Assert.AreEqual(res.Count(),1);
        }

        [Test]
        [TestCase("testUserId",1)]
        public async Task GetTaskById_TaskTypeTask_ReturnsSubTasksAndBaseTask(string userId,int taskId)
        {
            // Arrange
            var task = GetTasks().Where(x => x.Id == taskId).FirstOrDefault();
            var subTasks = GetTasks().Where(x=>x.BaseTaskId == taskId);

            _unitOfWork.Setup(x => x.TasksRepository.Get(It.IsAny<Expression<Func<Tasks, bool>>>()))
                       .ReturnsAsync(new List<Tasks> { task });
            _unitOfWork.Setup(x => x.TasksRepository.GetSubTasksForTask(task.Id))
                       .ReturnsAsync(subTasks);

            // Act
            var res = await _service.GetTaskById(userId, task.Id);

            // Assert
            Assert.AreEqual(taskId, res.Id);
            Assert.AreEqual(subTasks.Count(), res.SubTasks.Count());
        }

        [Test]
        [TestCase("testUserId",5)]
        public async Task GetTaskById_TaskTypeSubTask_ReturnsOnlyTaskRequested(string userId, int taskId)
        {
            // Arrange
            var task = GetTasks().Where(x => x.Id == taskId).FirstOrDefault();

            _unitOfWork.Setup(x => x.TasksRepository.Get(It.IsAny<Expression<Func<Tasks, bool>>>()))
                           .ReturnsAsync(new List<Tasks> { task });

            // Act
            var res = await _service.GetTaskById(userId, taskId);

            // Assert
            Assert.AreEqual(taskId, res.Id);

        }

        private IEnumerable<Tasks> GetTasks()
        {
            return new List<Tasks>
            {
                new Tasks { Id = 1, UserId = "testUserId",TaskType = TaskType.Task,Status = DAL.Models.TaskStatus.Done, Category = TaskCategory.Work},
                new Tasks { Id = 2, UserId = "wrongUserId", TaskType = TaskType.Task, Status = DAL.Models.TaskStatus.Done, Category = TaskCategory.Work},
                new Tasks { Id = 3, UserId = "testUserId", TaskType = TaskType.SubTask, Status = DAL.Models.TaskStatus.Done, Category = TaskCategory.Work, BaseTaskId = 2},
                new Tasks { Id = 4, UserId = "testUserId", TaskType = TaskType.Task, Status = DAL.Models.TaskStatus.Rejected, Category = TaskCategory.Work},
                new Tasks { Id = 5, UserId = "testUserId", TaskType = TaskType.SubTask, Status = DAL.Models.TaskStatus.Rejected, Category = TaskCategory.Work, BaseTaskId = 1},
                new Tasks { Id = 6, UserId = "testUserId", TaskType = TaskType.SubTask, Status = DAL.Models.TaskStatus.Rejected, Category = TaskCategory.Work, BaseTaskId = 1},
            };
        }
    }
}
