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
    /// <summary>
    /// Interface for Service B
    /// </summary>
    public class ServiceA : IServiceA
    {
        /// <summary>
        /// Client of Service B
        /// </summary>
        ServiceReferenceB.ServiceBClient srvB;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServiceA()
        {
            srvB = new ServiceReferenceB.ServiceBClient();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">Endpoint of WCF Service</param>
        public ServiceA(Uri uri = null)
        {
            srvB = new ServiceReferenceB.ServiceBClient();
            if (!(uri is null))
            {
                srvB.Endpoint.Address = new EndpointAddress(uri, srvB.Endpoint.Address.Identity, srvB.Endpoint.Address.Headers);
            }
        }

        /// <summary>
        /// This function verifies if conection to WCF Service A 
        /// is successful
        /// </summary>
        /// <returns>In case of connection on, return true.
        /// Otherwise, return false</returns>
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

        /// <summary>
        /// Get a list of patients of this service
        /// </summary>
        /// <returns>Return a list of patients</returns>
        public List<global::SharedLibray.Patient> getPatients()
        {
            return new List<SharedLibray.Patient> {
                new SharedLibray.Patient("A1", 1),
                new SharedLibray.Patient("A2", 2),
                new SharedLibray.Patient("A3", 3) };
        }

        /// <summary>
        /// Get the patients from the WCF Service B
        /// </summary>
        /// <returns>Returns a List of patients</returns>
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
