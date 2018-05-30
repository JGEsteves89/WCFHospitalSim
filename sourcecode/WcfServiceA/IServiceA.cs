using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceA
{
    /// <summary>
    /// Interface for Service B
    /// </summary>
   [ServiceContract]
    public interface IServiceA
    {
        /// <summary>
        /// Get a list of patients of this service
        /// </summary>
        /// <returns>Return a list of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> getPatients();

        /// <summary>
        /// Get the patients from the WCF Service B
        /// </summary>
        /// <returns>Returns a List of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> getPatientsFromB();

        /// <summary>
        /// This function verifies if conection to WCF Service B
        /// is successful
        /// </summary>
        /// <returns>In case of connection on, return true.
        /// Otherwise, return false</returns>
        [OperationContract]
        bool ConnectionOK();
        // TODO: Add your service operations here
    }

}
