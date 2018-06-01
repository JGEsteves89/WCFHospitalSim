using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WcfServiceBUnitTest
{
    [TestClass]
    public class UnitTestServiceB
    {
        /// <summary>
        /// Test the connection is OK to the Service A through service B
        /// </summary>
        [TestMethod]
        public void VerifyConnectionOK()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();
            Assert.AreEqual(true, srvB.ConnectionOK());
        }

        /// <summary>
        /// Test the connection is NOK to the Service A through service B
        /// </summary>
        [TestMethod]
        public void VerifyConnectionNOK()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB(new Uri("https://DOES_NOT_EXIST"));
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        #region Local 

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void CreateLocalPatient()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            Assert.IsNotNull(newPatient);
            Assert.AreNotEqual(newPatient.ID, 0);

            srvB.DeletePatient(newPatient);
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void ReadLocalPatient()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            SharedLibray.Patient readPatient = srvB.ReadPatient(newPatient.ID);

            Assert.IsNotNull(readPatient);
            Assert.AreNotEqual(readPatient.ID, 0);

            srvB.DeletePatient(newPatient);
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void UpdateLocalPatient()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            SharedLibray.Patient patient = srvB.ReadPatient(newPatient.ID);
            patient.Name = "Ricardo";
            patient.Age = 32;

            Assert.AreEqual(true, srvB.UpdatePatient(patient));

            srvB.DeletePatient(newPatient);
        }


        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void DeleteLocalPatient()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            Assert.AreEqual(true, srvB.DeletePatient(newPatient));
        }

        #endregion

        #region ServiceB 

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void CreatePatientFromA()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromA();

            Assert.IsNotNull(newPatient);
            Assert.AreNotEqual(newPatient.ID, 0);

            srvB.DeletePatientFromA(newPatient);
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void ReadLocalPatientFromA()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromA();

            SharedLibray.Patient readPatient = srvB.ReadPatientFromA(newPatient.ID);

            Assert.IsNotNull(readPatient);
            Assert.AreNotEqual(readPatient.ID, 0);

            srvB.DeletePatientFromA(newPatient);
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void UpdatePatientFromA()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromA();

            SharedLibray.Patient patient = srvB.ReadPatientFromA(newPatient.ID);
            patient.Name = "Ricardo";
            patient.Age = 32;

            Assert.AreEqual(true, srvB.UpdatePatientFromA(patient));

            srvB.DeletePatientFromA(newPatient);
        }


        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void DeletePatientFromA()
        {
            WcfServiceB.ServiceB srvB = new WcfServiceB.ServiceB();

            SharedLibray.Patient newPatient = srvB.CreatePatientFromA();

            Assert.AreEqual(true, srvB.DeletePatientFromA(newPatient));
        }

        #endregion
    }
}
