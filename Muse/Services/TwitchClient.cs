using Muse.Models.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Muse.Services
{
    public class TwitchClient
    {
        private HttpClient _client;
        public TwitchClient(HttpClient client)
        {
            _client = client;
        }

        public TokenResponse GetAppAccessToken()
        {
            _client.BaseAddress = new Uri("https://id.twitch.tv");
            _client.PostAsync("/oauth2/token?client_id=" + , new StringContent(""));

            return new TokenResponse();
        }
    }
}
