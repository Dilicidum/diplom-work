using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Interfaces;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    public class NotificationsController:ControllerBase
    {
        private readonly INotificationsService _notificationService;
        public NotificationsController(INotificationsService notificationService) 
        {
            _notificationService = notificationService;
        }

        [HttpGet("users/{userId}/notifications")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            if(userId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            var res = await _notificationService.GetNotifications(userId);

            return Ok(res);
        }
    }
}
