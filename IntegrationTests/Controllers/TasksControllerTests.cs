using Services.Abstractions.DTO;
using System.Text.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using IntegrationTests.Helpers;
using Domain.Entities;
using Infrastructure;
using Domain.Interfaces;
using Domain.Specifications;
using Service.Services;
using Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace IntegrationTests.Controllers
{
    [TestFixture]
    public class TasksControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private JsonSerializerOptions _jsonSerializerOptions;
        
        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            Arrange();
        }

        [Test]
        [TestCase("testUserId")]
        public async Task AddTask_InputIsValid_ReturnsCreatedAtActionAndCreatedTaskIsReturned(string userId)
        {
            // Arrange
            var taskInputModel = new TaskInputModel 
            {
                UserId = userId,
                BaseTaskId = null,
                Status = Domain.Entities.TaskStatus.Done,
                Category = Domain.Entities.TaskCategory.University,
                Description = "description",
                Name = "task name",
                DueDate = new DateTime(),
                TaskType = Domain.Entities.TaskType.Task,
            };

            // Act
            var json = JsonSerializer.Serialize(taskInputModel);
            var stringBody = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"users/{userId}/tasks", stringBody);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTask = JsonSerializer.Deserialize<Tasks>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(resultTask);
            Assert.AreEqual(taskInputModel.Name, resultTask?.Name);
            Assert.AreEqual(taskInputModel.Description, resultTask?.Description);
            Assert.AreEqual(taskInputModel.Status, resultTask?.Status);
            Assert.AreEqual(taskInputModel.Category, resultTask?.Category);
            Assert.AreEqual(taskInputModel.TaskType, resultTask?.TaskType);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task AddTask_InputIsInvalid_ReturnsBadRequest(string userId)
        {
            //Arrange
            var taskInputModel = new TaskInputModel { };

            //Act
            var json = JsonSerializer.Serialize(taskInputModel);
            var stringBody = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"users/{userId}/tasks", stringBody);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTask = JsonSerializer.Deserialize<Tasks>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("wrongUserId")]
        public async Task AddTask_UserDoesNotHaveAccess_ReturnsForbidden(string userId)
        {
            //Arrange
            var model = taskInputModel;

            //Act
            var json = JsonSerializer.Serialize(model);
            var stringBody = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"users/{userId}/tasks", stringBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_TaskTypeOnly_ReturnsTasksWithTypeBaseTask(string userId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks?taskType=Task");

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTasks = JsonSerializer.Deserialize<List<Tasks>>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, resultTasks?.Count());
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_TaskTypeAndTaskStatus_ReturnsTasksWithTypeBaseTaskAndStatusDone(string userId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks?taskType=Task&&status=Done");

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTasks = JsonSerializer.Deserialize<List<Tasks>>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, resultTasks?.Count());
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_TaskTypeAndTaskStatusAndTaskCategory_ReturnsTasksWithTypeBaseTaskAndStatusDoneAndCategoryWork(string userId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks?taskType=Task&&status=Done&&category=Work");

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTasks = JsonSerializer.Deserialize<List<Tasks>>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, resultTasks?.Count());
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_TaskTypeAndTaskCategory_ReturnsTasksWithTypeBaseTaskAndCategoryFitness(string userId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks?taskType=Task&&category=Fitness");

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTasks = JsonSerializer.Deserialize<List<Tasks>>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, resultTasks?.Count());
        }

        [Test]
        [TestCase("testUserId")]
        public async Task GetTasksWithFilter_InvalidFilters_ReturnsBadRequest(string userId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks?taskType=Invalid");

            // Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",1)]
        public async Task GetTaskById_TaskExists_ReturnsFoundTask(string userId,int taskId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks/{taskId}");

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var resultTask = JsonSerializer.Deserialize<Tasks>(result, _jsonSerializerOptions);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(taskId, resultTask?.Id);
            Assert.AreEqual(userId, resultTask?.UserId);
        }

        [Test]
        [TestCase("wrongUserId",1)]
        public async Task GetTaskById_UserDoesNotHaveAccess_ReturnsForbidden(string userId,int taskId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",1000)]
        public async Task GetTaskById_TaskNotFound_ReturnsNotFound(string userId,int taskId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("",1000)]
        public async Task GetTaskById_InvalidUserId_ReturnsNotFound(string userId,int taskId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",0)]
        public async Task GetTaskById_InvalidTaskId_ReturnsNotFound(string userId,int taskId)
        {
            //Act
            var response = await _client.GetAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",1)]
        public async Task Delete_TaskDeletionSuccessfull_ReturnsNoContent(string userId,int taskId)
        {
            //Act
            var response = await _client.DeleteAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        [TestCase("wrongUserId",1)]
        public async Task Delete_UserDoesNotHaveAccess_ReturnsForbidden(string userId,int taskId)
        {
            //Act
            var response = await _client.DeleteAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",1000)]
        public async Task Delete_TaskNotFound_ReturnsNotFound(string userId,int taskId)
        {
            //Act
            var response = await _client.DeleteAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("",1000)]
        public async Task Delete_InvalidUserId_ReturnsNotFound(string userId,int taskId)
        {
            //Act
            var response = await _client.DeleteAsync($"users/{userId}/tasks/{taskId}");

            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("testUserId",4)]
        public async Task UpdateTask_InvalidUserId_ReturnsNotFound(string userId,int taskId)
        {
            //Arrange
            var model = taskInputModel;
            model.Id = taskId;

            var json = JsonSerializer.Serialize(model);
            var stringBody = new StringContent(json, Encoding.UTF8, "application/json");


            //Act
            var response = await _client.PutAsync($"users/{userId}/tasks/{taskId}",stringBody);

            // Assert

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        private void Arrange()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();
                
                Utilities.InitializeDbForTests(db);
            }
        }

        private TaskInputModel taskInputModel =>  new TaskInputModel 
            {
                UserId = Utilities.testUserId,
                BaseTaskId = null,
                Status = Domain.Entities.TaskStatus.Done,
                Category = Domain.Entities.TaskCategory.University,
                Description = "description",
                Name = "task name",
                DueDate = new DateTime(),
                TaskType = Domain.Entities.TaskType.Task,
            };
    }
}
