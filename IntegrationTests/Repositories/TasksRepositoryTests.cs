using DAL;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class TasksRepositoryTests : BaseRepository
    {
        private ITasksRepository _repository;

        [SetUp]
        public override async Task SetUp()
        {
            await base.SetUp();
            await base.CreateUsers();
            await base.CreateTasks();
            _repository = new TasksRepository(_context);
        }

        [Test]
        public async Task GetDueTasksForToday_User1_ReturnsTaskForTodayForUser1()
        {
            // Act
            var res = await _repository.GetDueTasksForToday(user1Id);

            // Assert
            Assert.AreEqual(1, res.Count());
            Assert.IsTrue(res.All(t => t.DueDate == DateTime.Today && t.UserId == user1Id));
        }

        [Test]
        public async Task GetSubTasksForTask_ShouldReturnSubTasks()
        {
            // Act
            var subTasks = (await _repository.GetSubTasksForTask(1)).ToArray();

            // Assert
            Assert.AreEqual(1, subTasks.Count());
            Assert.IsTrue(subTasks.All(st => st.BaseTaskId == 1));
        }

        [TearDown] 
        public void Teardown() {
            _connection.Dispose();
        }
    }
}
