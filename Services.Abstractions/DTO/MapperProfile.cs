using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions.DTO;

namespace Services.Abstractions.DTO
{
    public class MapperProfile : Profile
    {
        public MapperProfile() { 
            CreateMap<UserRegistrationModel,IdentityUser>().ReverseMap();  
            CreateMap<UserLoginModel,IdentityUser>().ReverseMap();  
            CreateMap<TaskInputModel,Vacancy>().ReverseMap();
            CreateMap<CriteriaDto,Criteria>().ReverseMap();
            CreateMap<CandidateDTO,Candidate>().ReverseMap();
            CreateMap<CandidateCriteriaDTO,CandidateCriteria>().ReverseMap();
            CreateMap<AnalysisDTO,Analysis>().ReverseMap();
            CreateMap<Vacancy,Notification>()
                .ForMember(dest => dest.TaskId,act=>act.MapFrom(x=>x.Id))
                .ForMember(dest => dest.Title,act=>act.MapFrom(x=>x.Name))
                .ForMember(dest => dest.UserId,act=>act.MapFrom(x=>x.UserId));
            
        }
    }
}
