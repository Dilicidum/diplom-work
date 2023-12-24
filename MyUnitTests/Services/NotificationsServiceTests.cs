using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Service.Services;
using Services.Abstractions.Interfaces;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Services.Services;
using Services.Abstractions.DTO;
using Domain.Entities;
using Infrastructure;
using Domain.Interfaces;
using Domain.Specifications;

namespace UnitTests.Services
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class NotificationsServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private NotificationsService _service;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _service = new NotificationsService(_unitOfWork.Object,_mapper.Object);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetNotifications_NoTasksForToday_NoNotificationsReturned(string userId)
        {
            //Arrange
            _unitOfWork.Setup(x => x.TasksRepository.ListAsync(It.IsAny<TasksByUserIdAndDateSpec>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new List<Tasks> 
           { 
           });


            //Act
            var notifications = await _service.GetNotifications(userId);


            //Assert
            Assert.AreEqual(notifications.Count(),0);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetNotifications_TwoTasksForTodayExists_TwoNotificationsAreShowed(string userId)
        {
            _unitOfWork.Setup(x => x.TasksRepository.ListAsync(It.IsAny<TasksByUserIdAndDateSpec>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<Tasks> 
            {
            new Tasks
            {
                Id = 1,
                Description = "Desc1",
                Name= "Test1",
                DueDate = DateTime.Today,
                UserId = userId
            },
            new Tasks{
                Id = 2,
                Description = "Desc2",
                Name= "Test2",
                DueDate = DateTime.Today.AddDays(1),
                UserId = userId
            }
        }));

            _mapper.Setup(x => x.Map<Notification[]>(It.IsAny<IEnumerable<Tasks>>()))
            .Returns((IEnumerable<Tasks> tasks) => tasks.Select(x => new Notification { 
                TaskId  = x.Id,
                UserId = x.UserId,
                Title = x.Name
            }).ToArray());

            var service = new NotificationsService(_unitOfWork.Object,_mapper.Object);

            var notifications = await _service.GetNotifications(userId);

            Assert.AreEqual(notifications.Count(),2);
        }

    }
}
