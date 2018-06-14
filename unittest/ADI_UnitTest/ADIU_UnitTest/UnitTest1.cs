using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.ServiceProcess;

namespace ADIU_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        ADIU.IADIU iADIU = new ADIU.ADIUService();

        [TestMethod]
        public void TestGetAppointments()
        {
            int length = 0;
            length = iADIU.GetAppointments().Length;


            Assert.AreNotEqual(length, 0);
        }

        [TestMethod]
        public void TestGetAppointments_Null()
        {

            Assert.IsNull(iADIU.GetAppointments());
        }
    }
}
