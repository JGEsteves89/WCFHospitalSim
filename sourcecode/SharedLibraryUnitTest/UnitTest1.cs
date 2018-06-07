using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedLibray;

namespace SharedLibraryUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Verify if the values passed to the constructor are equal
        /// to the values return by the atributtes
        /// </summary>
        [TestMethod]
        public void NewPatientConstructorTest()
        {
            Patient newPatient = new Patient("Name", 2);

            Assert.AreEqual(newPatient.Name, "Name");
            Assert.AreEqual(newPatient.Age, 2);
        }
    }
}
