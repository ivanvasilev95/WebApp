using System.Linq;
using AutoMapper;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.DTOs.Message;
using WebBazar.API.DTOs.Photo;
using WebBazar.API.DTOs.User;
using WebBazar.API.DTOs.Category;
using WebBazar.API.Data.Models;

namespace WebBazar.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
         public AutoMapperProfiles() {
            CreateMap<Ad, AdForDetailedDTO>()
                .ForMember(dest => dest.Photos, opt => {
                    opt.MapFrom(src => src.Photos.Where(p => !p.IsDeleted).OrderByDescending(p => p.IsMain));
                })
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain && !p.IsDeleted).Url);
                })
                .ForMember(dest => dest.CategoryName, opt => {
                    opt.MapFrom(src => src.Category.Name);
                })
                .ForMember(dest => dest.UserName, opt => {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.LikesCount, opt => {
                    opt.MapFrom(src => src.Likes.Count());
                });

            CreateMap<Ad, AdForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain && !p.IsDeleted).Url);
                });

            CreateMap<AdForUpdateDTO, Ad>();
            CreateMap<AdForCreateDTO, Ad>();
            
            CreateMap<Category, CategoryToReturnDTO>();
            CreateMap<CategoryForCreationDTO, Category>();

            CreateMap<User, UserForDetailedDTO>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<User, UserForUpdateDTO>();
            CreateMap<UserForUpdateDTO, User>()
                .ForMember(dest => dest.NormalizedEmail, opt => {
                    opt.MapFrom(src => src.Email != null ?  src.Email.ToUpper() : null);
                });

            CreateMap<MessageForCreationDTO, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(dest => dest.SenderUsername, opt => {
                    opt.MapFrom(src => src.Sender.UserName);
                })
                .ForMember(dest => dest.RecipientUsername, opt => {
                    opt.MapFrom(src => src.Recipient.UserName);
                });

            CreateMap<Photo, PhotoForReturnDTO>();  
            CreateMap<Photo, PhotoForDetailedDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
         }
    }
}