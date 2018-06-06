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
        /// Repository of Patient in Server Database
        /// </summary>
        Hospital_BL.PatientRepository repository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServiceA()
        {
            srvB = new ServiceReferenceB.ServiceBClient();
            repository = new Hospital_BL.PatientRepositoryServer();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">Endpoint of WCF Service</param>
        public ServiceA(Uri uri = null)
        {
            srvB = new ServiceReferenceB.ServiceBClient();
            if (uri != null)
            {
                srvB.Endpoint.Address = new EndpointAddress(uri, srvB.Endpoint.Address.Identity, srvB.Endpoint.Address.Headers);
            }
        }

        #region CRUD

        /// <summary>
        /// Update Patient in remote database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        public bool UpdatePatient(global::SharedLibray.Patient patient)
        {
            try
            {
                return repository.Update(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///  Update Patient in Server Database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the update was sucessfull. Otherwise, returns false</returns>
        public bool UpdatePatientFromB(global::SharedLibray.Patient patient)
        {
            try
            {
                return srvB.UpdatePatient(patient);
     
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Delete Patient in server database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        public bool DeletePatient(SharedLibray.Patient patient)
        {
            try
            {
                return repository.Delete(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Delete Patient in remote database
        /// </summary>
        /// <param name="patient">Patient information</param>
        /// <returns>True if the delete was sucessfull. Otherwise, returns false</returns>
        public bool DeletePatientFromB(SharedLibray.Patient patient)
        {
            try
            {
                return srvB.DeletePatient(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Read Patient in server database
        /// </summary>
        /// <param name="ID">ID of patient</param>
        /// <returns>Return the information if exists, otherwise returns null</returns>
        public global::SharedLibray.Patient ReadPatient(ulong ID)
        {
            try
            {
                return repository.Read(ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Read Patient in remote Database
        /// </summary>
        /// <param name="ID">ID of patient</param>
        /// <returns>Return the information if exists, otherwise returns null</returns>
        public global::SharedLibray.Patient ReadPatientFromB(ulong ID)
        {
            try
            {
                return srvB.ReadPatient(ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Create Patient in server Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
        public global::SharedLibray.Patient CreatePatient()
        {
            try
            {
                return repository.Create();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Create Patient in remote Database
        /// </summary>
        /// <returns>If the patient is created successfull, returns a oject
        /// otherwise returns null</returns>
        public global::SharedLibray.Patient CreatePatientFromB()
        {
            try
            {
                return srvB.CreatePatient();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        #endregion

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
        public List<global::SharedLibray.Patient> GetPatients()
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
        public List<Patient> GetPatientsFromB()
        {
            try
            {
                // srvB = new ServiceReferenceB.ServiceBClient();
                List<global::SharedLibray.Patient> patients = srvB.GetPatients().ToList();
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
