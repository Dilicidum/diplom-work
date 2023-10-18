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

        public TasksController(ITasksService taskService, UserManager<IdentityUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Tasks task)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var user = await _userManager.FindByIdAsync(task.UserId);

            if(user!= null)
            {
                task.User = user;
            }

            await _taskService.AddTask(task);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            string userName = User.FindFirstValue(ClaimTypes.Name);
            
            if(userId == null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                userId = user.Id;
            }
            var tasks = (await _taskService.GetTasksForUser(userId)).ToList();

            return Ok(tasks);
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

            return Ok("Task was deleted");
        }
    }
}
