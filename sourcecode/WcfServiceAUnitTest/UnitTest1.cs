using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfServiceAUnitTest.ServiceReferenceA;

namespace WcfServiceAUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConnectionOk()
        {
            ServiceAClient newClient = new ServiceAClient();

            Assert.AreEqual(newClient.ConnectionOK(), true);
        }

        [TestMethod]
        public void ListPatientsFromBNotNull()
        {
            ServiceAClient newClient = new ServiceAClient();

            Assert.AreEqual(newClient.ConnectionOK(), true);
            Assert.IsNotNull(newClient.getPatientsFromB());
        }
    }
}
