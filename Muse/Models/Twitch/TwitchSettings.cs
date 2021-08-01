using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muse.Models.Twitch
{
    public class TwitchSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scope { get; set; }
    }
}
