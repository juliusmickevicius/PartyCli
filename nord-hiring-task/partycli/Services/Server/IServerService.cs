using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public interface IServerService
    {
        Task<string> GetAllServersListAsync();
        Task<string> GetAllServerByCountryListAsync(int countryId);
        Task<string> GetAllServerByProtocolListAsync(int vpnProtocol);
    }
}
