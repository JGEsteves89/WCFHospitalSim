using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SharedLibray;

namespace WcfServiceA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceA : IServiceA
    {
        ServiceReferenceB.ServiceBClient srvB;

        public ServiceA()
        {
            srvB = new ServiceReferenceB.ServiceBClient();
        }

        public ServiceA(Uri uri = null)
        {
            srvB = new ServiceReferenceB.ServiceBClient();
            if (!(uri is null))
            {
                srvB.Endpoint.Address = new EndpointAddress(uri, srvB.Endpoint.Address.Identity, srvB.Endpoint.Address.Headers);
            }

        }

        public bool ConnectionOK()
        {
            try
            {
                srvB.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public List<global::SharedLibray.Patient> getPatients()
        {
            return new List<SharedLibray.Patient> {
                new SharedLibray.Patient("A1", 1),
                new SharedLibray.Patient("A2", 2),
                new SharedLibray.Patient("A3", 3) };
        }

        public List<Patient> getPatientsFromB()
        {
            try
            {
                // srvB = new ServiceReferenceB.ServiceBClient();
                List<global::SharedLibray.Patient> patients = srvB.getPatients().ToList();
                srvB.Close();

                return patients;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
