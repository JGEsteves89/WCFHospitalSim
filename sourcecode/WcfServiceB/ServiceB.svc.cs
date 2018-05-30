using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceB
{
    /// <summary>
    /// Implementation of Interface 
    /// </summary>
    public class ServiceB : IServiceB
    {
        /// <summary>
        /// Client of Service A
        /// </summary>
        ServiceReferenceA.ServiceAClient srvA;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServiceB()
        {
            srvA = new ServiceReferenceA.ServiceAClient();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">Endpoint of WCF Service</param>
        public ServiceB(Uri uri=null)
        {
            srvA = new ServiceReferenceA.ServiceAClient();
            if (!(uri is null))
            {
                srvA.Endpoint.Address = new EndpointAddress(uri, srvA.Endpoint.Address.Identity, srvA.Endpoint.Address.Headers);
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
                srvA.Open();
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
                new SharedLibray.Patient("B1", 1),
                new SharedLibray.Patient("B2", 2),
                new SharedLibray.Patient("B3", 3) };
        }

        /// <summary>
        /// Get the patients from the WCF Service A
        /// </summary>
        /// <returns>Returns a List of patients</returns>
        public List<global::SharedLibray.Patient> getPatientsFromA()
        {
            try
            {
                List<global::SharedLibray.Patient> patients = srvA.getPatients().ToList() ;
                srvA.Close();

                return patients;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
