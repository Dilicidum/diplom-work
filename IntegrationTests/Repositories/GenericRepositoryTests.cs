using DAL.Models;
using DAL.Repositories;
using DAL;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using DAL.Interfaces;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class GenericRepositoryTests : BaseRepository
    {
        private IGenericRepository<Tasks> _tasksRepository;

        [SetUp]
        public override async Task SetUp()
        {
            await base.SetUp();
            await base.CreateUsers();
            await base.CreateTasks();
            _tasksRepository = new GenericRepository<Tasks>(_context);
        }

        [Test]
        public async Task AddTask_AddBaseTask_TaskAdded()
        {
            // Arrange
            var user = _userManager.Users.First();
            var task = new Tasks { DueDate = DateTime.Today, UserId = user.Id, Name = "1", Description = "3", BaseTaskId = null,TaskType = TaskType.Task };

            // Act
            await _tasksRepository.Add(task);
            await _context.SaveChangesAsync();

            // Assert
            var res = (await _tasksRepository.Get(t => t.Id == task.Id)).FirstOrDefault();
            Assert.IsNotNull(res);
        }

        [Test]
        [TestCase(1)]
        public async Task DeleteById_RemoveBaseTask_TaskRemoved(int taskId)
        {
            // Act
            await _tasksRepository.DeleteById(taskId);
            await _context.SaveChangesAsync();

            // Assert
            var res = await _tasksRepository.GetById(taskId);
            Assert.IsNull(res);
        }

        [Test]
        public async Task Get_GetBaseTasks_ReturnsAllBaseTasks()
        {
            // Act
            var tasks = await _tasksRepository.Get(x => x.TaskType == TaskType.Task);

            // Assert
            Assert.AreEqual(2,tasks.Count());
        }

        [Test]
        [TestCase(1)]
        public async Task GetById_TaskExists_ReturnsTask(int taskId)
        {
            // Act
            var res = await _tasksRepository.GetById(taskId);

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(taskId, res.Id);
        }


        [TearDown]
        public void TearDown()
        {
            _connection.Dispose();
        }


    }
}
