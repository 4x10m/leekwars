using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    public class Weapon
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "template")]
        public int template;
    }
}