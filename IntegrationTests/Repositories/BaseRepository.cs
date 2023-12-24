using DAL;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Primitives;

namespace IntegrationTests.Repositories
{
    public abstract class BaseRepository
    {
        protected SqliteConnection _connection;
        protected DbContextOptions<ApplicationContext> _contextOptions;
        protected UserManager<IdentityUser> _userManager;
        protected ApplicationContext _context;

        public BaseRepository()
        {

        }

        protected string user1Id { get
            {
                return "user1Id";
            } }

        protected string user2Id { get
            {
                return "user2Id";
            } }

        public virtual async Task SetUp()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            await _connection.OpenAsync();

            _contextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ApplicationContext(_contextOptions);
            await _context.Database.EnsureCreatedAsync();

            var userStore = new UserStore<IdentityUser>(new ApplicationContext(_contextOptions));
            _userManager = new UserManager<IdentityUser>(userStore, null, new PasswordHasher<IdentityUser>(), null, null, null, null, null, null);
        }

        protected virtual async Task CreateUsers()
        {
            var user1 = new IdentityUser {Id = user1Id, UserName = "user1", Email = "user1@gmail.com" };
            var user2 = new IdentityUser {Id = user2Id, UserName = "user2", Email = "user2@gmail.com" };
            await _userManager.CreateAsync(user1, "pass");
            await _userManager.CreateAsync(user2, "pass");
        }

        protected virtual async Task CreateTasks()
        {

            var tasks = new List<Tasks>()
            {
                new Tasks { Id = 1,DueDate = DateTime.Today, UserId = user1Id, Name = "1", Description = "3", BaseTaskId = null,TaskType = TaskType.Task },
                new Tasks { Id = 2,DueDate = DateTime.Today.AddDays(1), UserId = user1Id, Name = "2", Description = "3", TaskType = TaskType.SubTask, BaseTaskId = 1 },
                new Tasks { Id = 3,DueDate = DateTime.Today, UserId = user2Id, Name = user1Id, Description = "3", TaskType = TaskType.Task }
            };

            await _context.Tasks.AddRangeAsync(tasks);
            await _context.SaveChangesAsync();
        }
    }
}
