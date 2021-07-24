using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muse.Models.Twitch
{
    public class TwitchSettings
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string[] scope { get; set; }
    }
}
