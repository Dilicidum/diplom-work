using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService) {
            this.userService = userService;
        }

        [HttpGet]
        public List<UserDTO> GetUsers()
        {
            return this.userService.GetUsers();
        }
    }
}
