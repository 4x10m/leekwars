using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class Tournament
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "date")]
        public int date;

        [DataMember(Name = "registered")]
        public bool registered;

        [DataMember(Name = "current")]
        public string current;
    }
}