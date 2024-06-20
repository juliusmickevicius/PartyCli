using Microsoft.Extensions.Options;
using partycli.Settings;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public class ServerHttpService : IServersHttpService
    {
        private const string SERVERS_ENDPOINT = "/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=";
        private const string SERVERS_BY_COUNTRY_ENDPOINT = "/v1/servers?filters[servers_technologies][id]=";

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ServerHttpService(IOptions<ApiSettings> apiSettings)
        {
            _baseUrl = apiSettings.Value.NordVpnBaseUri;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAllServerByCountryListAsync(int? countryId = null)
        {
            return countryId is null
                ? await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, null)
                : await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, countryId);
        }

        public async Task<string> GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            return await SendGetRequestAsync(_baseUrl + SERVERS_BY_COUNTRY_ENDPOINT, vpnProtocol);
        }

        private async Task<string> SendGetRequestAsync(
            string requestUrl,
            int? value)
        {
            HttpRequestMessage request;

            if (string.IsNullOrWhiteSpace(requestUrl))
                throw new ArgumentNullException(requestUrl);

            request = value is null 
                ? new HttpRequestMessage(HttpMethod.Get, requestUrl)
                : new HttpRequestMessage(HttpMethod.Get, requestUrl + value);

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
