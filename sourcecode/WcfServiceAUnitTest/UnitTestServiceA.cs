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

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
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
        /// Test the connection is NOK to the Service B through service A
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
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void UpdateLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            SharedLibray.Patient patient = srvB.ReadPatient(newPatient.ID);
            patient.name = "Ricardo";
            patient.age = 32;

            Assert.AreEqual(true, srvB.UpdatePatient(patient));

            srvB.DeletePatient(newPatient);
        }


        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void DeleteLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

            SharedLibray.Patient newPatient = srvB.CreatePatient();

            Assert.AreEqual(true, srvB.DeletePatient(newPatient));
        }
    }
}
