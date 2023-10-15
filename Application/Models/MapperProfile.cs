using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile() { 
            CreateMap<UserRegistrationModel,IdentityUser>().ReverseMap();  
            CreateMap<UserLoginModel,IdentityUser>().ReverseMap();  
        }
    }
}
