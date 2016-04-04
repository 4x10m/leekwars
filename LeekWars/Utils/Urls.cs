using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeekWars.Utils
{
    public sealed class Urls
    {
        public static readonly Urls baseUrl = new Urls("http://leekwars.com");
        public static readonly Urls loginUrl = new Urls("http://leekwars.com/api/farmer/login-token");
        public static readonly Urls gardenUrl = new Urls("http://leekwars.com/api/garden/get");
        public static readonly Urls startSoloFightUrl = new Urls("http://leekwars.com/api/garden/start-solo-fight");
        public static readonly Urls privateTeamUrl = new Urls("http://leekwars.com/team/get-private");

        private Urls(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}