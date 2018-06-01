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
    /// Interface for Service B
    /// </summary>
    [ServiceContract]
    public interface IServiceB
    {
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
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatientFromA(global::SharedLibray.Patient patient);

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
        bool DeletePatientFromA(SharedLibray.Patient patient);

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
        global::SharedLibray.Patient ReadPatientFromA(ulong ID);

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
        global::SharedLibray.Patient CreatePatientFromA();

        /// <summary>
        /// Get the patients from the WCF Service A
        /// </summary>
        /// <returns>Returns a List of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> getPatientsFromA();

        /// <summary>
        /// Get a list of patients of this service
        /// </summary>
        /// <returns>Return a list of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> getPatients();

        /// <summary>
        /// This function verifies if conection to WCF Service A 
        /// is successful
        /// </summary>
        /// <returns>In case of connection on, return true.
        /// Otherwise, return false</returns>
        [OperationContract]
        bool ConnectionOK();
    }
}
