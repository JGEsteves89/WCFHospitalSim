using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIU
{
    public class WorkPatient
    {
        public WorkPatient()
        {
            StepID = null; // Scheduled procedure step ID
            StepDesc = null; // Scheduled procedure step description
            PhysicianName = null;
            ProcedureID = null; // Requested precedure ID
            ProcedureDesc = null; // Requested procedure description
            StudyInstance = null;
            Accession = null;
            PatientName = null;
            PatientID = null;
            PatientBirthDay = null;
            PatientSex = null;
        }

        public String StartDate
        {
            get;
            set;
        }
        public String StartTime
        {
            get;
            set;
        }
        public String Modality
        {
            get;
            set;
        }

        public String StepID // Scheduled procedure step ID
        {
            get;
            set;
        }

        public String StepDesc // Scheduled procedure step description
        {
            get;
            set;
        }
        public String PhysicianName
        {
            get;
            set;
        }
        // Requested precedure ID
        public String ProcedureID
        {
            get;
            set;
        }
        public String ProcedureDesc // Requested procedure description
        {
            get;
            set;
        }
        public String StudyInstance
        {
            get;
            set;
        }
        public String Accession
        {
            get;
            set;
        }
        public String PatientName
        {
            get;
            set;
        }
        public String PatientID
        {
            get;
            set;
        }
        public String PatientBirthDay
        {
            get;
            set;
        }
        public String PatientSex
        {
            get;
            set;
        }

        public override string ToString()
        {
            return String.Join(" ", PatientID, "\t" ,PatientName);
        }

       
    }
}
