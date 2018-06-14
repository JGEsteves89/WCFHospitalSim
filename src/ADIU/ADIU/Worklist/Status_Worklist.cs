using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace ADIU
{
    [DataContract(Name = "Status_Worklist")]
    public enum Status_Worklist
    {
        [EnumMember]
        IN_PROGRESS =0,
        [EnumMember]
        DISCONTINUED =1,
        [EnumMember]
        COMPLETED =3
    }
}
