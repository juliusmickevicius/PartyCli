using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public class ServerService : IServerService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetAllServersListAsync()
        {
            return await SendGetRequestAsync("https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=", null);
        }

        public async Task<string> GetAllServerByCountryListAsync(int countryId)
        {
            return await SendGetRequestAsync("https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=", countryId); ;
        }

        public async Task<string> GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            return await SendGetRequestAsync("https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=", vpnProtocol);
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
