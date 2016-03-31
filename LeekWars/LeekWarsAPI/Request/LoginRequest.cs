using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    class LoginRequest : LeekWarsRequest
    {
        [DataMember]
        public Farmer farmer;

        [DataMember]
        public string token;
    }
}