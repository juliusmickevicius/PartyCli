using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public interface IServersHttpService
    {
        Task<string> GetAllServerByCountryListAsync(int? countryId = null);
        Task<string> GetAllServerByProtocolListAsync(int vpnProtocol);
    }
}
