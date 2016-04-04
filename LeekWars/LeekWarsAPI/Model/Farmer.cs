using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI.Model
{
    [DataContract]
    public class Farmer
    {
        [DataMember]
        public int id;

        [DataMember]
        public string login;

        [DataMember]
        public Team team;

        [DataMember]
        public string name;

        [DataMember]
        public int talent;

        [DataMember]
        public Leek[] leeks;

        [DataMember(Name = "avatar_changed")]
        public int avatar;

        [DataMember(Name = "talent_more")]
        public int talentMore;

        [DataMember(Name = "victories")]
        public int victories;

        [DataMember(Name = "fights")]
        public Fight[] fights;

        [DataMember(Name = "draws")]
        public int draws;

        [DataMember(Name = "defeats")]
        public int defeats;

        [DataMember(Name = "ratio")]
        public float ratio;

        [DataMember(Name = "connected")]
        public bool connected;

        [DataMember(Name = "last_connection")]
        public int last_connection;

        [DataMember(Name = "register_date")]
        public int register_date;

        [DataMember(Name = "admin")]
        public bool admin;

        [DataMember(Name = "moderator")]
        public bool moderator;

        [DataMember(Name = "country")]
        public string country;

        [DataMember(Name = "godfather")]
        public string godfather;

        [DataMember(Name = "godsons")]
        public Object[] godsons;

        [DataMember(Name = "color")]
        public string color;

        [DataMember(Name = "banned")]
        public int banned;

        [DataMember(Name = "won_solo_tournaments")]
        public int won_solo_tournaments;

        [DataMember(Name = "won_farmer_tournaments")]
        public int won_farmer_tournaments;

        [DataMember(Name = "won_team_tournaments")]
        public int won_team_tournaments;

        [DataMember(Name = "habs")]
        public int habs;

        [DataMember(Name = "crystals")]
        public int crystals;

        [DataMember(Name = "weapons")]
        public Weapon[] weapons;

        [DataMember(Name = "chips")]
        public Chip[] chips;

        [DataMember(Name = "ais")]
        public AI[] ais;

        [DataMember(Name = "potions")]
        public Potion[] potions;

        [DataMember(Name = "hats")]
        public string[] hats;

        [DataMember(Name = "tournament")]
        public Tournament tournament;

        [DataMember(Name = "tournaments")]
        public Tournament[] tournaments;

        [DataMember(Name = "candidacy")]
        public string candidacy;

        [DataMember(Name = "total_level")]
        public int candidacytotal_level;

        [DataMember(Name = "leek_count")]
        public int leek_count;
    }
}
