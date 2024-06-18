using Microsoft.Extensions.Options;
using partycli.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public class ServerService : IServerService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string SERVERS_ENDPOINT = "/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=";
        private const string SERVERS_BY_COUNTRY = "/v1/servers?filters[servers_technologies][id]=";

        private readonly string _baseUrl;

        public ServerService(IOptions<ApiSettings> apiSettings)
        {
            _baseUrl = apiSettings.Value.NordVpnBaseUri;
        }

        public async Task<string> GetAllServersListAsync()
        {
            return await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, null);
        }

        public async Task<string> GetAllServerByCountryListAsync(int countryId)
        {
            return await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, countryId); ;
        }

        public async Task<string> GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            return await SendGetRequestAsync(_baseUrl + SERVERS_BY_COUNTRY, vpnProtocol);
        }

        private async Task<string> SendGetRequestAsync(
            string requestUrl,
            int? value)
        {
            HttpRequestMessage request;

            if (string.IsNullOrWhiteSpace(requestUrl))
                throw new ArgumentNullException(requestUrl);

            if (value is null)
            {
                request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, requestUrl + value);
            }

            var response = await client.SendAsync(request);
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
    }
}
