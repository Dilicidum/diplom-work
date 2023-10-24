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

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TasksController:ControllerBase
    {
        private ITasksService _taskService;
        private UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public TasksController(ITasksService taskService, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _taskService = taskService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskInputModel task)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(task.UserId != userId)
            {
                return BadRequest("Invalid user");
            }

            var taskToCreate = _mapper.Map<Tasks>(task);
            
            await _taskService.AddTask(taskToCreate);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksWithFilter([FromQuery]DAL.Models.TaskStatus? status, [FromQuery]TaskType? taskType)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = (await _taskService.GetTasksForUser(userId)).ToList();
            
            Func<Tasks, bool> filter = x => 
            (!status.HasValue || x.Status == status.Value) && 
            (!taskType.HasValue || x.TaskType == taskType.Value);

            var filteredTasks = await _taskService.GetTasksForUser(userId,filter);

            return Ok(filteredTasks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = (await _taskService.GetTasksForUser(userId,x=>x.Id == Id)).SingleOrDefault();

            if(task == null)
            {
                return BadRequest("Task not found");
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
                return BadRequest("Task not found");
            }

            await _taskService.DeleteTask(task);

            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateTask(TaskInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = _mapper.Map<Tasks>(model);

            await _taskService.UpdateTask(task);

            return Ok();
        }
    }
}
