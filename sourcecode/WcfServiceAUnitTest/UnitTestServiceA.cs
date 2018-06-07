using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WcfServiceAUnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UnitTestServiceA
    {
        /// <summary>
        /// Test the connection is OK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void TestConnectionOk()
        {
            WcfServiceA.ServiceA newClient = new WcfServiceA.ServiceA();

            Assert.AreEqual(true, newClient.ConnectionOK());
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void VerifyConnectionNOK()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA(new Uri("https://DOES_NOT_EXIST"));
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        #region Local 

        /// <summary>
        ///  Test the creation of Patient in local server
        /// </summary>
        [TestMethod]
        public void CreateLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            Assert.IsNotNull(newPatient);
            Assert.AreNotEqual(newPatient.ID, 0);

            srvB.DeletePatient(newPatient);
        }

        /// <summary>
        /// Test the read of Patient information in local server
        /// </summary>
        [TestMethod]
        public void ReadLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            SharedLibray.Patient readPatient = srvB.ReadPatient(newPatient.ID);

            Assert.IsNotNull(readPatient);
            Assert.AreNotEqual(readPatient.ID, 0);

            srvB.DeletePatient(newPatient);
        }

        /// <summary>
        /// Test the update of Patient information in local server
        /// </summary>
        [TestMethod]
        public void UpdateLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            SharedLibray.Patient patient = srvB.ReadPatient(newPatient.ID);
            patient.Name = "Ricardo";
            patient.Age = 32;

            Assert.AreEqual(true, srvB.UpdatePatient(patient));

            srvB.DeletePatient(newPatient);
        }


        /// <summary>
        /// Test the delete of Patient information in local server
        /// </summary>
        [TestMethod]
        public void DeleteLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            Assert.AreEqual(true, srvB.DeletePatient(newPatient));
        }

        #endregion

        #region ServiceB 

        /// <summary>
        /// Test the creation of Patient in remote server
        /// </summary>
        [TestMethod]
        public void CreatePatientFromB()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromB();

            Assert.IsNotNull(newPatient);
            Assert.AreNotEqual(newPatient.ID, 0);

            srvB.DeletePatientFromB(newPatient);
        }

        /// <summary>
        /// Test the read of Patient information in remote server
        /// </summary>
        [TestMethod]
        public void ReadPatientFromB()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromB();

            SharedLibray.Patient readPatient = srvB.ReadPatientFromB(newPatient.ID);

            Assert.IsNotNull(readPatient);
            Assert.AreNotEqual(readPatient.ID, 0);

            srvB.DeletePatientFromB(newPatient);
        }

        /// <summary>
        /// Test the update of Patient information in remote server
        /// </summary>
        [TestMethod]
        public void UpdatePatientFromB()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromB();

            SharedLibray.Patient patient = srvB.ReadPatientFromB(newPatient.ID);
            patient.Name = "Ricardo";
            patient.Age = 32;

            Assert.AreEqual(true, srvB.UpdatePatientFromB(patient));

            srvB.DeletePatientFromB(newPatient);
        }


        /// <summary>
        /// Test the delete of Patient information in remote server
        /// </summary>
        [TestMethod]
        public void DeletePatientFromB()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromB();

            Assert.AreEqual(true, srvB.DeletePatientFromB(newPatient));
        }

        #endregion
    }
}
