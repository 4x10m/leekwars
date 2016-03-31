using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class Team
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public int level;

        [DataMember]
        public string emblem_changed;

        [DataMember]
        public int xp;

        [DataMember]
        public int up_xp;

        [DataMember]
        public int down_xp;

        [DataMember]
        public int talent;

        [DataMember]
        public int victories;

        [DataMember]
        public Fight[] fights;

        [DataMember]
        public int draws;

        [DataMember]
        public int defeats;

        [DataMember]
        public string description;

        [DataMember]
        public int remaining_xp;

        [DataMember]
        public float ratio;

        [DataMember]
        public int member_count;

        [DataMember]
        public Member[] members;

        [DataMember]
        public int leek_count;

        [DataMember]
        public bool opened;

        [DataMember]
        public Tournament[] tournaments;

        [DataMember]
        public Object[] candidacies;

        [DataMember]
        public int forum;

        [DataMember]
        public Composition[] compositions;

        [DataMember]
        public Leek[] unengaged_leeks;
    }
}