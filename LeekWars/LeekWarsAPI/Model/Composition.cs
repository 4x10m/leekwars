using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class Composition
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "name")]
        public string name;

        [DataMember(Name = "total_level")]
        public int total_level;

        [DataMember(Name = "talent")]
        public int talent;

        [DataMember(Name = "fights")]
        public int fights;

        [DataMember(Name = "team")]
        public Team team;

        [DataMember(Name = "leeks")]
        public Leek[] leeks;

        [DataMember(Name = "tournament")]
        public Tournament tournament;
    }
}