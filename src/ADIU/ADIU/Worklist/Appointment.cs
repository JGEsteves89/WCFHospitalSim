using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace ADIU
{
    [DataContract]
    public class Appointment
    {

        [DataMember]
        public String StartDate
        {
            get;
            set;
        }

        [DataMember]
        public String StartTime
        {
            get;
            set;
        }

        [DataMember]
        public String Modality
        {
            get;
            set;
        }

        // Scheduled procedure step ID
        [DataMember]
        public String StepID
        {
            get;
            set;
        }

        // Scheduled procedure step description
        [DataMember]
        public String StepDesc
        {
            get;
            set;
        }

        [DataMember]
        public String PhysicianName
        {
            get;
            set;
        }

        // Requested precedure ID
        [DataMember]
        public String ProcedureID
        {
            get;
            set;
        }

        // Requested procedure description
        [DataMember]
        public String ProcedureDesc
        {
            get;
            set;
        }

        [DataMember]
        public String StudyInstance
        {
            get;
            set;
        }

        [DataMember]
        public String Accession
        {
            get;
            set;
        }

        [DataMember]
        public String PatientName
        {
            get;
            set;
        }

        [DataMember]
        public String PatientID
        {
            get;
            set;
        }

        [DataMember]
        public String PatientBirthDay
        {
            get;
            set;
        }

        [DataMember]
        public String PatientSex
        {
            get;
            set;
        }

        public override string ToString()
        {
            return String.Join(" ", PatientID, "\t", PatientName);
        }
    }
}
