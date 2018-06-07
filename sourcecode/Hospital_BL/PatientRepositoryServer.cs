using SharedLibray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_BL
{
    /// <summary>
    /// 
    /// </summary>
    public class PatientRepositoryServer:PatientRepository
    {
        HospitalDBEntities dBEntities;

        /// <summary>
        /// 
        /// </summary>
        public PatientRepositoryServer()
        {
            dBEntities = new HospitalDBEntities();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public override bool Update(SharedLibray.Patient patient)
        {           
            try
            {
                PatientInfo patient2DB = dBEntities.PatientInfoes.First(r => r.ID == (int) patient.ID);

                patient2DB.Name = patient.Name;
                patient2DB.Age = patient.Age;

                dBEntities.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public override bool Delete(SharedLibray.Patient patient)
        {
            try
            {
                PatientInfo patient2DB = dBEntities.PatientInfoes.First(r => r.ID == (int)patient.ID);

                dBEntities.PatientInfoes.Remove(patient2DB);
                dBEntities.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override SharedLibray.Patient Read(ulong ID)
        {
            try
            {
                PatientInfo patient2DB = dBEntities.PatientInfoes.First(r => r.ID == (int)ID);

                Patient patient = new Patient
                {
                    ID = (ulong)patient2DB.ID,
                    Age = patient2DB.Age,
                    Name = patient2DB.Name
                };

                return patient;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SharedLibray.Patient Create()
        {
            try
            {
                PatientInfo patient = new PatientInfo()
                {
                    Age = 0,
                    Name = string.Empty
                };

                dBEntities.PatientInfoes.Add(patient);
                dBEntities.SaveChanges();

                Patient newPatient = new Patient()
                {
                    Age = patient.Age,
                    Name = patient.Name,
                    ID = (ulong) patient.ID
                };

                return newPatient;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<SharedLibray.Patient> GetPatients()
        {
            try
            {
                List<PatientInfo> patientList = dBEntities.PatientInfoes.ToList();
                List<Patient> patients = new List<Patient>();

                foreach(PatientInfo info in patientList)
                {
                    Patient newPatient = new Patient
                    {
                        ID = (ulong)info.ID,
                        Age = info.Age,
                        Name = info.Name
                    };

                    patients.Add(newPatient);
                }
                
                return patients;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
