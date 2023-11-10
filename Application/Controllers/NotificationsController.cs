using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController:ControllerBase
    {
        private readonly ITasksService _taskService;
        private readonly IMapper _mapper;
        public NotificationsController(ITasksService tasksService,IMapper mapper) 
        {
            _taskService = tasksService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            string userIdToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userIdToken != userId)
            {
                return BadRequest();
            }

            var tasks = (await _taskService.GetTasksForUser(userIdToken,x => x.DueDate == DateTime.Today));

            var res = _mapper.Map<Notification[]>(tasks);

            return Ok(res);
        }
    }
}
