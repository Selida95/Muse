using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Muse.Models.Twitch
{
    public class TokenResponse
    {
        [Key]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
#nullable enable
        public string? RefreshToken { get; set; }
#nullable disable
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("scope")]
        public string[] Scope { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
