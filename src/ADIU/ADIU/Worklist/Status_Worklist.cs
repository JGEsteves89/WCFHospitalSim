using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace ADIU
{
    /// <summary>
    /// Enumeration for the Status worlist sent to
    /// the provider 
    /// </summary>
    [DataContract(Name = "Status_Worklist")]
    public enum Status_Worklist
    {
        /// <summary>
        /// In Progress
        /// </summary>
        [EnumMember]
        IN_PROGRESS =0,

        /// <summary>
        /// Discontinued
        /// </summary>
        [EnumMember]
        DISCONTINUED =1,

        /// <summary>
        /// Completed
        /// </summary>
        [EnumMember]
        COMPLETED =2
    }
}
