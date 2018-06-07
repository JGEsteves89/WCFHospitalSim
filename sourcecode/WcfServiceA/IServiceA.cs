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
        /// Update Patient in remote database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool UpdatePatientFromB(global::SharedLibray.Patient patient);

        /// <summary>
        /// Update Patient in Server Database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool UpdatePatient(global::SharedLibray.Patient patient);

        /// <summary>
        /// Delete Patient in server database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool DeletePatient(SharedLibray.Patient patient);

        /// <summary>
        /// Delete Patient in remote database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        [OperationContract]
        bool DeletePatientFromB(SharedLibray.Patient patient);

        /// <summary>
        /// Read Patient in server database
        /// </summary>
        /// <param name="ID">ID of patient</param>
        /// <returns>Return the information if exists, otherwise returns null</returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatient(ulong ID);

        /// <summary>
        /// Read Patient in remote Database
        /// </summary>
        /// <param name="ID">ID of patient</param>
        /// <returns>Return the information if exists, otherwise returns null</returns>
        [OperationContract]
        global::SharedLibray.Patient ReadPatientFromB(ulong ID);

        /// <summary>
        /// Create Patient in server Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
        [OperationContract]
        global::SharedLibray.Patient CreatePatient();

        /// <summary>
        /// Create Patient in remote Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
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
    }

}
