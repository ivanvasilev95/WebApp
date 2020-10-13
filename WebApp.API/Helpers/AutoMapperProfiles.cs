using System.Linq;
using AutoMapper;
using WebApp.API.Models;
using WebApp.API.DTOs.Ad;
using WebApp.API.DTOs.Message;
using WebApp.API.DTOs.Photo;
using WebApp.API.DTOs.User;
using WebApp.API.DTOs.Category;

namespace WebApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
         public AutoMapperProfiles() {
            CreateMap<Ad, AdForDetailedDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.CategoryName, opt => {
                    opt.MapFrom(src => src.Category.Name);
                });

            CreateMap<Ad, AdForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                });
            
            CreateMap<Category, CategoryToReturnDTO>();
            CreateMap<CategoryForCreationDTO, Category>();

            CreateMap<AdForUpdateDTO, Ad>();
            CreateMap<AdForCreateDTO, Ad>();
            
            CreateMap<User, UserForDetailedDTO>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<UserForUpdateDTO, User>()
                .ForMember(dest => dest.NormalizedEmail, opt => {
                    opt.MapFrom(src => src.Email != null ?  src.Email.ToUpper() : null);
                });

            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(dest => dest.SenderUsername, opt => {
                    opt.MapFrom(src => src.Sender.UserName);
                })
                .ForMember(dest => dest.RecipientUsername, opt => {
                    opt.MapFrom(src => src.Recipient.UserName);
                });
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();

            CreateMap<Photo, PhotoForReturnDTO>();  
            CreateMap<Photo, PhotoForDetailedDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
         }
    }
}