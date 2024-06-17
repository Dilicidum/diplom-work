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
    public class VacanciesController:ControllerBase
    {
        private readonly IVacanciesService _taskService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public VacanciesController(IVacanciesService taskService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _taskService = taskService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("users/{userId}/tasks")]
        public async Task<IActionResult> AddTask(string userId,TaskInputModel task)
        {

            var taskToCreate = _mapper.Map<Vacancy>(task);
            var i = 0;
            taskToCreate.Criterias.ForEach(x =>
            {
                x.Order = i;
                i++;
            });
            await _taskService.AddVacancy(taskToCreate);

            return Ok();
        }

        [HttpGet("users/{userId}/tasks")]
        public async Task<IActionResult> GetTasksWithFilter(string userId,[FromQuery]TaskType taskType,[FromQuery]TaskStatus? status, [FromQuery] TaskCategory? category, [FromQuery] string name="" )
        {
            if (name == "5")
            {
                name="";
            }
            var spec = new VacancyByTypeAndStatusAndCategorySpecAndUserId(name,taskType,category,status);

            var filteredTasks = await _taskService.GetVacanciesForUser(spec);

            return Ok(filteredTasks);
        }

        [HttpGet("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> GetVacancyById(string userId,int taskId)
        {
       

            var task = await _taskService.GetVacancyById(userId,taskId);

            if(task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }


        [HttpDelete("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> Delete(string userId,int taskId)
        {
            var task = (await _taskService.GetVacancyById(userId,taskId));

            if(task == null)
            {
                return NotFound();
            }

            await _taskService.DeleteVacancy(task);

            return NoContent();
        }

        [HttpPut("users/{userId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateVacancy(string userId,int taskId,TaskInputModel model)
        {
            var task = await _taskService.GetVacancyById(userId,taskId);

            if(task == null)
            {
                return NotFound();
            }
                
            _mapper.Map(model,task); 

            await _taskService.UpdateVacancy(task);

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
