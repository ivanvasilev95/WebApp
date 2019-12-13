using System;
using System.Linq;
using AutoMapper;
using WebApp.API.DTOs;
using WebApp.API.Models;

namespace WebApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
         public AutoMapperProfiles()
         {
            CreateMap<Ad, AdForDetailedDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            });

            CreateMap<Ad, AdForListDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            });
            
            CreateMap<Photo, PhotosForDetailedDTO>(); // PhotoForDetailedDTO
            CreateMap<User, UserForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<Ad, AdForDetailedDTO>();
            CreateMap<AdForUpdateDTO, Ad>();
            CreateMap<AdForCreateDTO, Ad>();
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDTO>();
         }
    }
}