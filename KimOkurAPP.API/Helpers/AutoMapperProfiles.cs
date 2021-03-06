using AutoMapper;
using KimOkur.API.Models;
using KimOkur.API.Dtos;
using System.Linq;
using KimOkurAPP.API.Dtos;
using KimOkurAPP.API.Models;

namespace KimOkur.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserListForDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault().Url))
            .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>().ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault().Url))
            .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<User,UserIdentityForUpdateDto>();
            CreateMap<UserIdentityForUpdateDto,User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo,PhotoForReturnDto>();
            CreateMap<PhotoForReturnDto,Photo>();
            CreateMap<UserForRegisterDto,User>();
            CreateMap<MessageForCreationDto,Message>().ReverseMap();
            CreateMap<Message,MessageToReturnDto>()
            .ForMember(m =>m.SenderPhotoUrl,opt=>opt
                    .MapFrom(u =>u.Sender.Photos.FirstOrDefault(p =>p.IsMain).Url))
            .ForMember(m =>m.RecipientPhotoUrl,opt=>opt
                    .MapFrom(u =>u.Recipient.Photos.FirstOrDefault(p =>p.IsMain).Url));
        }
    }
}