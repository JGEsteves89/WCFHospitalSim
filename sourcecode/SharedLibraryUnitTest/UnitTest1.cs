using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibray;

namespace SharedLibraryUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NewPatientConstructorTest()
        {
            Patient newPatient = new Patient("Name", 2);

            Assert.AreEqual(newPatient.name, "Name");
            Assert.AreEqual(newPatient.age, 2);
        }
    }
}
