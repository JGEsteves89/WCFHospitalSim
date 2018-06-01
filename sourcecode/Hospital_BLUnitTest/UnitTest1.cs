using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital_BL;
using SharedLibray;
using System.Collections.Generic;

namespace Hospital_BLUnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CreatePatientTest()
        {
            PatientRepository repository = new PatientRepository();
            Patient newPatient = repository.Create();

            Assert.AreNotEqual(0, newPatient.ID);

            repository.Delete(newPatient);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ReadPatientTest()
        {
            PatientRepository repository = new PatientRepository();
            Patient newPatient = repository.Read(1);

            Assert.IsNotNull(newPatient);
            Assert.AreNotEqual(0, newPatient.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UpdatePatientTest()
        {
            PatientRepository repository = new PatientRepository();
            Patient patient2Updated = repository.Read(1);
            patient2Updated.name = "Ricardo";
            patient2Updated.age = 32;

            Assert.AreEqual(true, repository.Update(patient2Updated));

            Patient patientUpdated = repository.Read(1);

            Assert.AreEqual(patient2Updated.ID, patientUpdated.ID);
            Assert.AreEqual(patient2Updated.age, patientUpdated.age);
            Assert.AreEqual(patient2Updated.name, patientUpdated.name);

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void DeletePatientTest()
        {
            PatientRepository repository = new PatientRepository();
            Patient newPatient = repository.Create();

            Assert.AreEqual(true,repository.Delete(newPatient));

            Assert.IsNull(repository.Read(newPatient.ID));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetListPatientsTest()
        {
            PatientRepository repository = new PatientRepository();

            var patients2DB = new List<Patient>();
            Patient newPatient1 = repository.Create();
            patients2DB.Add(newPatient1);
            Patient newPatient2 = repository.Create();
            patients2DB.Add(newPatient2);

            var patients = repository.GetPatients();

            CollectionAssert.AreEqual(patients2DB, patients,new PatientComparer());

            patients2DB.ForEach(r => repository.Delete(r));
        }

        public class PatientComparer : Comparer<Patient>
        {
            public override int Compare(Patient x, Patient y)
            {
                if (x.name.Equals(y.name) && x.ID == y.ID && x.age == y.age)
                    return 0;
                else
                    return -1;
            }
        }
    }
}
