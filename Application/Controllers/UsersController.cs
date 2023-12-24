using Services.Abstractions.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UsersController(UserManager<IdentityUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [Authorize(Policy = "CanSeeUsers")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var res = await _userManager.FindByIdAsync(userId);
            
            if(res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [Authorize(Policy = "CanSeeUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "CanAddUsers")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegistrationModel user)
        {
            var identityUser = _mapper.Map<IdentityUser>(user);
            var res = await _userManager.CreateAsync(identityUser,user.Password);

            if (!res.Succeeded)
            {
                return BadRequest();
            }
            
            var role = await _roleManager.FindByNameAsync(user.Role);

            if (role != null)
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser,role.Name);

                if (!roleResult.Succeeded)
                {
                    return BadRequest();
                }
            }
            
            return CreatedAtAction(nameof(GetUserById),new {Id = identityUser.Id },identityUser);
        }
    }
}
