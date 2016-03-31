using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class AI
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "name")]
        public string name;

        [DataMember(Name = "level")]
        public int level;
    }
}