using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    class Garden
    {
        [DataMember(Name = "leeks")]
        public Leek[] leeks;

        [DataMember(Name = "farmer_enabled")]
        public bool farmer_enabled;

        [DataMember(Name = "team_enabled")]
        public bool team_enabled;

        [DataMember(Name = "solo_fights")]
        public int solo_fights;

        [DataMember(Name = "solo_total_fights")]
        public int solo_total_fights;

        [DataMember(Name = "farmer_fights")]
        public int farmer_fights;

        [DataMember(Name = "team_fights")]
        public int team_fights;

        [DataMember(Name = "team_total_fights")]
        public int team_total_fights;

        [DataMember(Name = "my_compositions")]
        public Composition[] my_compositions;

        [DataMember(Name = "farmer_enemies")]
        public Farmer[] farmer_enemies;

        [DataMember(Name = "farmer_total_fights")]
        public int farmer_total_fights;

        [DataMember(Name = "my_farmer")]
        public Farmer my_farmer;

        [DataMember(Name = "my_team")]
        public Team my_team;
    }
}