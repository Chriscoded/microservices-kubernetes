using System.Threading.Tasks;
using PlatformService.Dtos;
namespace PlatformService.SyncDataServices.Http
{
    public interface HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        public HttpCommandDataClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task SendPlatformToCommand(PlatformReadDto plat)
        {
            throw new System.NotImplementedException();
        }
    } 
}