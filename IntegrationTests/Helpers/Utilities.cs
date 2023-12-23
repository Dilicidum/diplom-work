using Domain.Entities;
using Infrastructure;
using Domain.Interfaces;
using Domain.Specifications;
using Service.Services;
using Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationContext db)
        {
            db.Database.EnsureCreated();
            db.Tasks.AddRange(GetTasks());
            db.Users.AddRange(GetUsers());
            db.SaveChanges();
        }

        public static string testUserId { get
            {
                return "testUserId";
            } }

        public static string wrongUserId { get
            {
                return "wrongUserId";
            } }

        public static void ReinitializeDbForTests(ApplicationContext db)
        {
            db.Tasks.RemoveRange(db.Tasks);
            db.Users.RemoveRange(db.Users);
            
            InitializeDbForTests(db);
        }

        public static List<IdentityUser> GetUsers()
        {
            return new List<IdentityUser>()
            {
                new IdentityUser { Id = testUserId, UserName = "user1", Email = "user1@gmail.com" },
                new IdentityUser { Id = wrongUserId, UserName = "user2", Email = "user2@gmail.com" }
            };
        }

        public static List<Tasks> GetTasks()
        {
            return new List<Tasks>()
            {
                new Tasks { Id = 1,DueDate = DateTime.Today, UserId = testUserId, Name = "1", Description = "3", BaseTaskId = null,TaskType = TaskType.Task, Status = Domain.Entities.TaskStatus.None, Category = TaskCategory.Fitness },
                new Tasks { Id = 2,DueDate = DateTime.Today.AddDays(1), UserId = testUserId, Name = "2", Description = "3", TaskType = TaskType.SubTask, BaseTaskId = 1 },
                new Tasks { Id = 3,DueDate = DateTime.Today, UserId = wrongUserId, Name = testUserId, Description = "3", TaskType = TaskType.Task },
                new Tasks { Id = 4,DueDate = DateTime.Today.AddDays(1), UserId = testUserId, Name = "4", Description = "4", TaskType = TaskType.Task, BaseTaskId = null, Status = Domain.Entities.TaskStatus.Done, Category = TaskCategory.Work },
                new Tasks { Id = 5,DueDate = DateTime.Today.AddDays(1), UserId = testUserId, Name = "5", Description = "5", TaskType = TaskType.Task, BaseTaskId = null, Status = Domain.Entities.TaskStatus.Progress, Category = TaskCategory.Fitness },
            };
        }
    }
}
