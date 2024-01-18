using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType){
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:

                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default :
                Console.WriteLine("--> could not determine event type");
                    return EventType.Undetermined;
            }
        }
         private void addPlatform(string platformPublishedMessage){
            using (var scope = _scopeFactory.CreateScope()){
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExists(plat.ExternalID)){
                        Console.WriteLine("Creating new platform");
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                         Console.WriteLine("Platform added successfully");
                    }
                    else
                    {
                        Console.WriteLine("Platform already exist");
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Could not add platform DB {ex} ");
                }
            }
        }
    }

   
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}