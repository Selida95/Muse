using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Muse.Data;
using Muse.Models.Twitch;
using Newtonsoft.Json;
using System;
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
        private IDbContextFactory<DataContext> _factory;
        public TwitchClient(HttpClient client, IOptions<TwitchSettings> settings, IDbContextFactory<DataContext> factory)
        {
            _client = client;
            _settings = settings.Value;
            _factory = factory;
        }

        public async Task<TokenResponse> RequestAppAccessToken()
        {
            _client.BaseAddress = new Uri("https://id.twitch.tv");
            var response = await _client.PostAsync($"/oauth2/token?client_id={_settings.ClientId}&client_secrets={_settings.ClientSecret}&grant_type=client_credentials&{string.Join("%20", _settings.Scope)}", new StringContent(""));

            return JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task SaveAccessToken(TokenResponse token)
        {
            using DataContext context = _factory.CreateDbContext();
            context.TwitchTokens.Add(token);
            await context.SaveChangesAsync();
        }
    }
}
