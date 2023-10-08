using BLL.DTO;
using BLL.Interfaces;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext applicationContext;
        public UserService(ApplicationContext applicationContext) {
            this.applicationContext = applicationContext;
        }

        public List<UserDTO> GetUsers()
        {
            var users = applicationContext.Users.ToList();

            var usersDTO = users.Select(x=> new UserDTO{
                Id = x.Id,
                Name = x.Name,
                Email = x.Email}).ToList();

            return usersDTO;
        }
    }
}
