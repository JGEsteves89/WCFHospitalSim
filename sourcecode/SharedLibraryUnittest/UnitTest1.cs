using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibray;

namespace SharedLibraryUnittest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NewPatientConstructor()
        {
            Patient newPatient = new Patient("Name", 2);
            Assert.AreEqual("Name", newPatient.name);
            Assert.AreEqual(2, newPatient.age);
        }
    }
}
