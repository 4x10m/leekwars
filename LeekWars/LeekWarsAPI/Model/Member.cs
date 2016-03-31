using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LeekWars.LeekWarsAPI.Model
{
    [DataContract]
    public class Member
    {
        [DataMember]
        int id;

        [DataMember]
        string name;

        [DataMember]
        int avatar_changed;

        [DataMember]
        string grade;

        [DataMember]
        bool connected;
    }
}
