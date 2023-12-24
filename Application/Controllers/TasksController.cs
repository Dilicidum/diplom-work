using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Services.Abstractions.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Services.Abstractions.Interfaces;
using Domain.Entities;
using Domain.Specifications;
using TaskStatus = Domain.Entities.TaskStatus;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    public class TasksController:ControllerBase
    {
        private readonly ITasksService _taskService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksController(ITasksService taskService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _taskService = taskService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("users/{userId}/tasks")]
        public async Task<IActionResult> AddTask(string userId,TaskInputModel task)
        {
            if (!HasAccess(userId))
            {
                return Forbid();
            }

            var taskToCreate = _mapper.Map<Tasks>(task);
            
            await _taskService.AddTask(taskToCreate);

            return CreatedAtAction(nameof(GetTaskById),new {userId = taskToCreate.UserId,taskId = taskToCreate.Id}, taskToCreate);
        }

        [HttpGet("users/{userId}/tasks")]
        public async Task<IActionResult> GetTasksWithFilter(string userId,[FromQuery]TaskType taskType,[FromQuery]TaskStatus? status, [FromQuery] TaskCategory? category )
        {
            if (!HasAccess(userId))
            {
                return Forbid();
            }

            var spec = new TasksByTypeAndStatusAndCategorySpecAndUserId(userId,taskType,category,status);

            var filteredTasks = await _taskService.GetTasksForUser(spec);

            return Ok(filteredTasks);
        }

        [HttpGet("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(string userId,int taskId)
        {
            if (!HasAccess(userId))
            {
                return Forbid();
            }

            var task = await _taskService.GetTaskById(userId,taskId);

            if(task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }


        [HttpDelete("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> Delete(string userId,int taskId)
        {
            if (!HasAccess(userId))
            {
                return Forbid();
            }

            var task = (await _taskService.GetTaskById(userId,taskId));

            if(task == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTask(task);

            return NoContent();
        }

        [HttpPut("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(string userId,int taskId,TaskInputModel model)
        {
            if (!HasAccess(userId))
            {
                return Forbid();
            }

            var task = await _taskService.GetTaskById(userId,taskId);

            if(task == null)
            {
                return NotFound();
            }
                
            _mapper.Map(model,task); 

            await _taskService.UpdateTask(task);

            return NoContent();
        }

        private bool HasAccess(string userId)
        {
            if(!(userId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin")))
            {
                return false;
            }

            return true;
        }
    }
}
