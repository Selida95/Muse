using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Muse.Models.Twitch
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string accessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string? refreshToken { get; set; }
        [JsonProperty("expires_in")]
        public int expiresIn { get; set; }
        [JsonProperty("scope")]
        public string[] scope { get; set; }
        [JsonProperty("token_type")]
        public string tokenType { get; set; }
    }
}
