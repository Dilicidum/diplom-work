using API.Models;
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


        [HttpGet("{userId}")]
        public async Task<IdentityUser> GetUserById(string userId)
        {
            var res = await _userManager.FindByIdAsync(userId);
            
            return res;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var res = await _userManager.Users.ToListAsync();

            return res;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegistrationModel user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Model you provide is invalid, please try again");
            }

            var identityUser = _mapper.Map<IdentityUser>(user);
            var res = await _userManager.CreateAsync(identityUser,user.Password);

            if (!res.Succeeded)
            {
                return BadRequest("Error when creating user");
            }
            
            var role = await _roleManager.FindByNameAsync(user.Role);

            if (role != null)
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser,role.Name);

                if (!roleResult.Succeeded)
                {
                    return BadRequest("Role adding failed");
                }
            }
            
            return Ok("User was created");
        }
    }
}
