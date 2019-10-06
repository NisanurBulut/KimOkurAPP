using AutoMapper;
using KimOkur.API.Models;
using KimOkur.API.Dtos;
using System.Linq;
using KimOkurAPP.API.Dtos;

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
        }
    }
}