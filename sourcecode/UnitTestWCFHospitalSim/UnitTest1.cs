using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestWCFHospitalSim
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSCU()
        {
            SCP.SCPClient target = new SCP.SCPClient();
            int expected = DateTime.Now.Hour;
            Assert.AreEqual(expected, target.GetListOfPatients());

        }
    }
}
