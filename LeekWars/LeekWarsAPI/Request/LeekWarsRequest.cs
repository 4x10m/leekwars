using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI
{
    [DataContract]
    class LeekWarsRequest
    {
        [DataMember]
        public bool success;

        //{"success":false,"error":"wrong_token"}
    }
}
