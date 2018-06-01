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
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatientFromB(global::SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatient(global::SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePatient(SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePatientFromB(SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatient(ulong ID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatientFromB(ulong ID);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        global::SharedLibray.Patient CreatePatient();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        global::SharedLibray.Patient CreatePatientFromB();


        /// <summary>
        /// Get a list of patients of this service
        /// </summary>
        /// <returns>Return a list of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> GetPatients();

        /// <summary>
        /// Get the patients from the WCF Service B
        /// </summary>
        /// <returns>Returns a List of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> GetPatientsFromB();

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
