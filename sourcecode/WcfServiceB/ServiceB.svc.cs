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
        /// 
        /// </summary>
        Hospital_BL.PatientRepository repository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServiceB()
        {
            srvA = new ServiceReferenceA.ServiceAClient();
            repository = new Hospital_BL.PatientRepositoryLocal();
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
            repository = new Hospital_BL.PatientRepositoryServer();
        }



        #region CRUD

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool UpdatePatientFromA(global::SharedLibray.Patient patient)
        {
            try
            {
                return srvA.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public bool DeletePatientFromA(SharedLibray.Patient patient)
        {
            try
            {
                return srvA.DeletePatient(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public global::SharedLibray.Patient ReadPatientFromA(ulong ID)
        {
            try
            {
                return srvA.ReadPatient(ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public global::SharedLibray.Patient CreatePatientFromA()
        {
            try
            {
                return srvA.CreatePatient();
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
        public List<global::SharedLibray.Patient> GetPatients()
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
        public List<global::SharedLibray.Patient> GetPatientsFromA()
        {
            try
            {
                List<global::SharedLibray.Patient> patients = srvA.GetPatients().ToList();
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
