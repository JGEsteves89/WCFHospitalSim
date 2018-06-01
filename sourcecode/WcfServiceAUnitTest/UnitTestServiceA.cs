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

        #region Local Database

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void CreateLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();

         //   SharedLibray.Patient patient = srvB.CreatePatient();

          //  Assert.IsNotNull(patient);
        //    Assert.AreNotEqual(patient.ID,0);

         //   srvB.DeletePatient(patient);
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void ReadLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA();
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void UpdateLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA(new Uri("https://DOES_NOT_EXIST"));
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        /// <summary>
        /// Test the connection is NOK to the Service B through service A
        /// </summary>
        [TestMethod]
        public void DeleteLocalPatient()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA(new Uri("https://DOES_NOT_EXIST"));
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        #endregion

        #region Server Database

        #endregion
    }
}
