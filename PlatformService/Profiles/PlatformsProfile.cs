using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    //class inherit Profile from AutoMapper
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
                // -> Source -> Target
                //platform is source while plateformReadDto is target
                CreateMap<Platform, PlatformReadDto>();
                CreateMap<PlatformCreateDto, Platform>();
                CreateMap<PlatformReadDto, PlatformPublishedDto>();
        }
    }
}
