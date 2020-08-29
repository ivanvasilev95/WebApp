using System.Linq;
using AutoMapper;
using WebApp.API.DTOs.Ad;
using WebApp.API.DTOs.Message;
using WebApp.API.DTOs.Photo;
using WebApp.API.DTOs.User;
using WebApp.API.Models;

namespace WebApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
         public AutoMapperProfiles() {
            CreateMap<Ad, AdForDetailedDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            });

            CreateMap<Ad, AdForListDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            });
            
            CreateMap<Photo, PhotoForDetailedDTO>();
            CreateMap<User, UserForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<AdForUpdateDTO, Ad>();
            CreateMap<AdForCreateDTO, Ad>();
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDTO>();
            CreateMap<Photo, PhotoForReturnDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
         }
    }
}