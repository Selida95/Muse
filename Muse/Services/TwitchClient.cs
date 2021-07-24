using Microsoft.Extensions.Options;
using Muse.Models.Twitch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Muse.Services
{
    public class TwitchClient
    {
        private HttpClient _client;
        private TwitchSettings _settings;
        public TwitchClient(HttpClient client, IOptions<TwitchSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }

        public async Task<TokenResponse> GetAppAccessToken()
        {
            _client.BaseAddress = new Uri("https://id.twitch.tv");
            var response = await _client.PostAsync($"/oauth2/token?client_id={_settings.clientId}&client_secrets={_settings.clientSecret}&grant_type=client_credentials&{string.Join("%20", _settings.scope)}", new StringContent(""));

            return JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
