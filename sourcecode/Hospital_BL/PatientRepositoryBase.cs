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
    public abstract class PatientRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public PatientRepository()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public abstract bool Update(SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public abstract bool Delete(SharedLibray.Patient patient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract SharedLibray.Patient Read(ulong ID);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract SharedLibray.Patient Create();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract List<SharedLibray.Patient> GetPatients();
    }
}
