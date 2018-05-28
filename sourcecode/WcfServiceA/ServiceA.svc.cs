using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PatientDLL;

namespace WcfServiceA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceA : IServiceA
    {
        public List<global::PatientDLL.Patient> getPatients()
        {
            return new List<PatientDLL.Patient> {
                new PatientDLL.Patient("A1", 1),
                new PatientDLL.Patient("A2", 2),
                new PatientDLL.Patient("A3", 3) };
        }
        public List<Patient> getPatientsFromB()
        {
            throw new NotImplementedException();
        }
    }
}
