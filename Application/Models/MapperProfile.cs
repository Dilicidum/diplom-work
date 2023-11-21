using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using BLL.Models;

namespace API.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile() { 
            CreateMap<UserRegistrationModel,IdentityUser>().ReverseMap();  
            CreateMap<UserLoginModel,IdentityUser>().ReverseMap();  
            CreateMap<TaskInputModel,Tasks>().ReverseMap();
            CreateMap<Tasks,Notification>()
                .ForMember(dest => dest.TaskId,act=>act.MapFrom(x=>x.Id))
                .ForMember(dest => dest.Title,act=>act.MapFrom(x=>x.Name))
                .ForMember(dest => dest.UserId,act=>act.MapFrom(x=>x.UserId));
        }
    }
}
