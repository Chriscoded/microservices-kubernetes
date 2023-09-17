using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Model;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repository;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper,  
            ICommandDataClient commandDataClient)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
        }

        //GET /api/platform
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms...");
            var platformItem = repository.GetAllPlatforms();

            //mapping object of platform model to PlatformReadDto

            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        //GET /api/platform/id
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = repository.GetPlatformById(id);
            //mapping object of platform model to PlatformReadDto
            if(platformItem != null)
            {
                return Ok(mapper.Map<PlatformReadDto>(platformItem));
            }
           
                return NotFound();
            
        }

        //GET /api/platform/id
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatPlatform(PlatformCreateDto platformCreateDto)
        {
            //mapping object of PlatformCreatDto to platform model
            var platformModel = mapper.Map<Platform>(platformCreateDto);
            repository.CreatePlatform(platformModel);
            repository.SaveChanges();
            //return Ok(repository.GetPlatformById(platformModel.Id));
            //mapping object of platform model to PlatformReadDto
            var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);

             // Send Sync Message
            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //CreatedAtRoute returns http 201 with route as part of the return
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
