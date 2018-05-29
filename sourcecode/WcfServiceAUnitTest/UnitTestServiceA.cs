using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WcfServiceAUnitTest
{
    [TestClass]
    public class UnitTestServiceA
    {
        [TestMethod]
        public void TestConnectionOk()
        {
            WcfServiceA.ServiceA newClient = new WcfServiceA.ServiceA();

            Assert.AreEqual(true, newClient.ConnectionOK());
        }

        [TestMethod]
        public void VerifyConnectionNOK()
        {
            WcfServiceA.ServiceA srvB = new WcfServiceA.ServiceA(new Uri("https://DOES_NOT_EXIST"));
            Assert.AreEqual(false, srvB.ConnectionOK());
        }

        [TestMethod]
        public void ListPatientsFromBNotNull()
        {
            WcfServiceA.ServiceA newClient = new WcfServiceA.ServiceA();

            Assert.IsNotNull(newClient.getPatientsFromB());
        }

        [TestMethod]
        public void ListPatientsNotNull()
        {
            WcfServiceA.ServiceA newClient = new WcfServiceA.ServiceA();

            Assert.IsNotNull(newClient.getPatients());
        }
    }
}
