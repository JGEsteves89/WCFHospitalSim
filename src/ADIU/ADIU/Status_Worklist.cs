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
        IN_PROGRESS =0,
        DISCONTINUED=1,
        COMPLETED=2
    }
}
