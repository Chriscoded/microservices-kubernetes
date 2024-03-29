﻿using AutoMapper;
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
                CreateMap<Platform, GrpcPlatformModel>()
                    .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
