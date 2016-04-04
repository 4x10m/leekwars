using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI.Model
{
    [DataContract]
    public class Leek
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public string color;

        [DataMember]
        public int capital;

        [DataMember]
        public int level;

        [DataMember]
        public int talent;

        [DataMember]
        public int skin;

        [DataMember]
        public string hat;

        [DataMember]
        public int fights;

        [DataMember]
        public Leek[] ennemies;
    }
}