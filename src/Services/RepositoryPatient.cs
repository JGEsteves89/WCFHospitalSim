using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class  RepositoryPatient
    {
        public static List<Patient> GetListPatients()
        {
            List<Patient> patients = new List<Patient>();

            patients.Add(new Patient() { ID = 1, Age = 18, Name = "Paulo 1" });
            patients.Add(new Patient() { ID = 1, Age = 19, Name = "Paulo 2" });
            patients.Add(new Patient() { ID = 1, Age = 20, Name = "Paulo 3" });
            patients.Add(new Patient() { ID = 1, Age = 21, Name = "Paulo 4" });
            patients.Add(new Patient() { ID = 1, Age = 22, Name = "Paulo 5" });
            patients.Add(new Patient() { ID = 1, Age = 23, Name = "Paulo 6" });
            patients.Add(new Patient() { ID = 1, Age = 24, Name = "Paulo 7" });
            patients.Add(new Patient() { ID = 1, Age = 25, Name = "Paulo 8" });
            patients.Add(new Patient() { ID = 1, Age = 26, Name = "Paulo 9" });
            patients.Add(new Patient() { ID = 1, Age = 27, Name = "Paulo 10" });

            return patients;
        }
              
    }
}
