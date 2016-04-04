using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using LeekWars.LeekWarsAPI.Model;

namespace LeekWars.LeekWarsAPI.Request
{
    [DataContract]
    class GardenRequest : LeekWarsRequest
    {
        [DataMember]
        public Garden garden;
    }
}