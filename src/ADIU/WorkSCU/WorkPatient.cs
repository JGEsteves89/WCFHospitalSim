using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSCU
{
    public class WorkPatient
    {

        public String startDate = null;
        public String startTime = null;
        public String modality = null;
        public String stepID = null; // Scheduled procedure step ID
        public String stepDesc = null; // Scheduled procedure step description
        public String physicianName = null;
        public String procedureID = null; // Requested precedure ID
        public String procedureDesc = null; // Requested procedure description
        public String studyInstance = null;
        public String accession = null;
        public String patientName = null;
        public String patientID = null;
        public String patientBirthDay = null;
        public String patientSex = null;

        public WorkPatient() : base()
        {
        }
        public override string ToString()
        {
            return String.Join(" ", patientID, patientName, patientSex, patientBirthDay, studyInstance, startDate, startTime);
        }
    }
}
