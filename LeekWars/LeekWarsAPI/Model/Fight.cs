using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class Fight
    {
        [DataMember(Name ="id")]
        public int id;

        [DataMember(Name = "date")]
        public int date;

        [DataMember(Name = "type")]
        public int type;

        [DataMember(Name = "context")]
        public int context;

        [DataMember(Name = "status")]
        public int status;

        [DataMember(Name = "winner")]
        public int winner;

        [DataMember(Name = "farmer_team")]
        public int farmer_team;

        [DataMember(Name = "result")]
        public string result;

        [DataMember(Name = "farmer1")]
        public int farmer1;

        [DataMember(Name = "farmer2")]
        public int farmer2;

        [DataMember(Name = "farmer1_name")]
        public string farmer1_name;

        [DataMember(Name = "statusfarmer2_name")]
        public string farmer2_name;

        [DataMember(Name = "team1")]
        public int team1;

        [DataMember(Name = "team2")]
        public int team2;

        [DataMember(Name = "team1_name")]
        public string team1_name;

        [DataMember(Name = "team2_name")]
        public string team2_name;
    }
}