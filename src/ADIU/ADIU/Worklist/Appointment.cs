using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace ADIU
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Appointment
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
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
        /// 
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
        ///  Requested procedure description
        /// </summary>
        [DataMember]
        public String ProcedureDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String StudyInstance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Accession
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PatientName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PatientID
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PatientBirthDay
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PatientSex
        {
            get;
            set;
        }
    }
}
