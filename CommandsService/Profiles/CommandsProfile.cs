using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformreadDto>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto,Command>();
        }
    }
}