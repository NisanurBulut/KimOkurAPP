using AutoMapper;
using DatingAPP.API.Models;
using DatingAPP.API.Dtos;
using System.Linq;

namespace DatingAPP.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserListForDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
              opt.MapFrom(src => src.Photos.FirstOrDefault().Url));
            CreateMap<User, UserForDetailedDto>().ForMember(dest => dest.PhotoUrl, opt =>
              opt.MapFrom(src => src.Photos.FirstOrDefault().Url));
            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}