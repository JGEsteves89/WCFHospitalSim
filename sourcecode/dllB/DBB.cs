using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientDLL;

namespace dllB
{
    public class DBB
    {
        private List<Patient> patients = new List<Patient>();
        public DBB()
        {
            patients.Add(new Patient("A", 1));
            patients.Add(new Patient("B", 2));
            patients.Add(new Patient("C", 3));
            patients.Add(new Patient("D", 4));
            patients.Add(new Patient("E", 5));
            patients.Add(new Patient("F", 6));
        }
        public List<Patient> GetPatients()
        {
            return this.patients;
        }
    }
}
