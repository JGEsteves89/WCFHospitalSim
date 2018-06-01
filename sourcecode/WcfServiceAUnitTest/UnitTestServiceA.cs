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
    }
}
