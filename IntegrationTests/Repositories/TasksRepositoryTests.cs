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
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using Infrastructure.Repositories;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class TasksRepositoryTests : BaseRepository
    {
        private ITasksRepository _repository;

        [SetUp]
        public async Task Setup()
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
