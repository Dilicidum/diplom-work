using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
namespace API.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile() { 
            CreateMap<UserRegistrationModel,IdentityUser>().ReverseMap();  
            CreateMap<UserLoginModel,IdentityUser>().ReverseMap();  
            CreateMap<TaskInputModel,Tasks>().ReverseMap();  
        }
    }
}
