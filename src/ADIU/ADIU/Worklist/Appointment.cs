using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Globalization;


namespace ADIU
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Appointment
    {
        /// <summary>
        /// Start Time of Procedure
        /// Format Type: yyyyMMdd
        /// </summary>
        [DataMember]
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Start Time of Procedure
        /// It's not datetime type
        /// </summary>
        [DataMember]
        public String StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Modality
        /// </summary>
        [DataMember]
        public String Modality
        {
            get;
            set;
        }

        /// <summary>
        /// Scheduled procedure step ID
        /// </summary>
        [DataMember]
        public String StepID
        {
            get;
            set;
        }

        /// <summary>
        /// Scheduled procedure step description
        /// </summary>
        [DataMember]
        public String StepDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Physician Name
        /// </summary>
        [DataMember]
        public String PhysicianName
        {
            get;
            set;
        }

        /// <summary>
        /// Requested precedure ID
        /// </summary>
        [DataMember]
        public String ProcedureID
        {
            get;
            set;
        }

        /// <summary>
        /// Requested procedure description
        /// </summary>
        [DataMember]
        public String ProcedureDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Study Instance
        /// </summary>
        [DataMember]
        public String StudyInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Access Number 
        /// </summary>
        [DataMember]
        public String Accession
        {
            get;
            set;
        }

        /// <summary>
        /// Patient Name
        /// </summary>
        [DataMember]
        public String PatientName
        {
            get;
            set;
        }

        /// <summary>
        /// Patient Identification
        /// </summary>
        [DataMember]
        public String PatientID
        {
            get;
            set;
        }

        /// <summary>
        /// Patient Birth Day
        /// </summary>
        [DataMember]
        public String PatientBirthDay
        {
            get;
            set;
        }

        /// <summary>
        /// Patient Sex
        /// </summary>
        [DataMember]
        public PatientSex PatientSex
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Enumeration of Patient Sex
    /// </summary>
    [DataContract(Name = "PatientSex")]
    public enum PatientSex
    {
        /// <summary>
        /// Male
        /// </summary>
        [EnumMember]
        Male = 0,

        /// <summary>
        /// Female
        /// </summary>
        [EnumMember]
        Female = 1,

        /// <summary>
        /// Other gender
        /// </summary>
        [EnumMember]
        NotDefined = 2
    }
}
