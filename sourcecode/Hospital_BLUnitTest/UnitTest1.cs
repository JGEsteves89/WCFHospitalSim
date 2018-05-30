using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital_BL;
using SharedLibray;

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
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ReadPatientTest()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UpdatePatientTest()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void DeletePatientTest()
        {
        }
    }
}
