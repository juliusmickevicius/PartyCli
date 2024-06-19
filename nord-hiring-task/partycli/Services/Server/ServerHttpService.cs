﻿using Microsoft.Extensions.Options;
using partycli.Settings;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Services.Server
{
    public class ServerHttpService : IServersHttpService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string SERVERS_ENDPOINT = "/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=";
        private const string SERVERS_BY_COUNTRY_ENDPOINT = "/v1/servers?filters[servers_technologies][id]=";

        private readonly string _baseUrl;

        public ServerHttpService(IOptions<ApiSettings> apiSettings)
        {
            _baseUrl = apiSettings.Value.NordVpnBaseUri;
        }

        public async Task<string> GetAllServerByCountryListAsync(int? countryId = null)
        {
            if (countryId == null)
            {
                return await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, null);
            }
            else
            {
                return await SendGetRequestAsync(_baseUrl + SERVERS_ENDPOINT, countryId);
            }
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

            if (value is null)
            {
                request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, requestUrl + value);
            }

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}