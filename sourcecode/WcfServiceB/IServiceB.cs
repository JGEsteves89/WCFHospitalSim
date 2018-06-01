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
        /// Update Patient in local database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool UpdatePatient(global::SharedLibray.Patient patient);

        /// <summary>
        /// Update Patient in Server Database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool UpdatePatientFromA(global::SharedLibray.Patient patient);

        /// <summary>
        /// Delete Patient in local database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool DeletePatient(SharedLibray.Patient patient);

        /// <summary>
        /// Delete Patient in server database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool DeletePatientFromA(SharedLibray.Patient patient);

        /// <summary>
        /// Read Patient in local database
        /// </summary>
        /// <param name="ID">ID of patient</param>
        /// <returns>Return the information if exists, otherwise returns false</returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatient(ulong ID);

        /// <summary>
        /// Read Patient in server Database
        /// </summary>
        /// <param name="ID">ID of Patient</param>
        /// <returns>Return the information if exists, otherwise returns false</returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatientFromA(ulong ID);

        /// <summary>
        /// Create Patient in local Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
        [OperationContract]
        global::SharedLibray.Patient CreatePatient();

        /// <summary>
        /// Create Patient in server Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
        [OperationContract]
        global::SharedLibray.Patient CreatePatientFromA();

        /// <summary>
        /// Get the patients from the WCF Service A
        /// </summary>
        /// <returns>Returns a List of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> GetPatientsFromA();

        /// <summary>
        /// Get a list of patients of this service
        /// </summary>
        /// <returns>Return a list of patients</returns>
        [OperationContract]
        List<global::SharedLibray.Patient> GetPatients();

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
