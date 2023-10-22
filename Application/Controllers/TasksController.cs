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

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return BadRequest("Invalid user");
            }


            var taskToCreate = _mapper.Map<Tasks>(task);
            taskToCreate.UserId = userId;

            await _taskService.AddTask(taskToCreate);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksWithFilter([FromQuery]DAL.Models.TaskStatus? status)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(status == null)
            {
                var tasks = (await _taskService.GetTasksForUser(userId)).ToList();

                return Ok(tasks);
            }

            var filteredTasks = await _taskService.GetTasksForUser(userId,(x=>x.Status == status));

            return Ok(filteredTasks);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string userName = User.FindFirstValue(ClaimTypes.Name);
            
            if(userId == null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                userId = user.Id;
            }

            var task = (await _taskService.GetTasksForUser(userId)).FirstOrDefault(x=>x.Id == Id);

            if(task == null)
            {
                return BadRequest();
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
