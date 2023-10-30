using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DAL.Specifications;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TasksController:ControllerBase
    {
        private ITasksService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITasksService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskInputModel task)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(task.UserId != userId)
            {
                return BadRequest("Invalid user");
            }

            var taskToCreate = _mapper.Map<Tasks>(task);
            
            await _taskService.AddTask(taskToCreate);

            return CreatedAtAction(nameof(GetTaskById),new {Id = taskToCreate.Id}, taskToCreate);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksWithFilter([FromQuery]TaskType taskType,[FromQuery]DAL.Models.TaskStatus? status, [FromQuery] TaskCategory? category )
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TaskTypeSpecification taskTypeSpecification = new TaskTypeSpecification(taskType);
            Specification<Tasks> specification = taskTypeSpecification;
            
            if(status.HasValue)
            {
                specification = specification.AndSpecification(new StatusSpecification(status.Value));
            }

            if(category.HasValue)
            {
                specification = specification.AndSpecification(new TaskCategorySpecification(category.Value));
            }

            var filteredTasks = await _taskService.GetTasksForUser(userId, specification);

            return Ok(filteredTasks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = (await _taskService.GetTasksForUser(userId,x=>x.Id == Id)).SingleOrDefault();

            if(task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = (await _taskService.GetTasksForUser(userId)).FirstOrDefault(x=>x.Id == Id);

            if(task == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTask(task);

            return NoContent();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateTask(int Id,TaskInputModel model)
        {
            if(Id != model.Id)
            {
                return BadRequest();
            }

            var taskExists = await _taskService.ValidateTaskExistence(model.Id);

            if(!taskExists)
            {
                return NotFound();
            }

            var task = _mapper.Map<Tasks>(model);

            await _taskService.UpdateTask(task);

            return NoContent();
        }
    }
}
