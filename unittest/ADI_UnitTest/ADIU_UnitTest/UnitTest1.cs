using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Diagnostics;

namespace ADIU_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        ADIU.IADIU iADIU = new ADIU.ADIUService();

        [TestMethod]
        public void TestSSL()
        {
            ADIU.MergeHandler handler = ADIU.MergeHandler.Instance;
            handler.GetConfiguration();
            handler.SecureAssociation = true;

            ADIU.IADIU iADIUssl = new ADIU.ADIUService(handler);
            int length = 0;
            length = iADIU.GetAppointments().Length;
            Assert.AreNotEqual(length, 0);
        }

        [TestMethod]
        public void TestGetAppointments()
        {
            int length = 0;
            length = iADIU.GetAppointments().Length;


            Assert.AreNotEqual(length, 0);
        }


        [TestMethod]
        public void TestCreateStatusAppointtmentPatient()
        {
            List<ADIU.Appointment> list = new List<ADIU.Appointment>();
            list.AddRange(iADIU.GetAppointments());

            ADIU.Appointment appointment = list[list.Count - 1];
            string affectedSOP = iADIU.CreateStatusAppointments(appointment);

            Assert.IsNotNull(affectedSOP);
        }

        [TestMethod]
        public void TestCreateStatusNullPatient()
        {
            string affectedSOP = iADIU.CreateStatusAppointments(null);

            Assert.IsNull(affectedSOP);
        }


        [TestMethod]
        public void TestCreateStatusPatientNotexist()
        {
            ADIU.Appointment appointment = new ADIU.Appointment();
            string affectedSOP = iADIU.CreateStatusAppointments(appointment);

            Assert.IsNull(affectedSOP);
        }

        [TestMethod]
        public void TestSetStatusInProgress()
        {
            List<ADIU.Appointment> list = new List<ADIU.Appointment>();
            list.AddRange(iADIU.GetAppointments());
            ADIU.Appointment appointment = list[list.Count - 1];

            string affectedSOP = iADIU.CreateStatusAppointments(appointment);

            Assert.IsTrue(iADIU.SetStatusAppointments(affectedSOP, ADIU.StatusWorklist.InProgress));
        }

        [TestMethod]
        public void TestSetStatusInProgressNull()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(null, ADIU.StatusWorklist.InProgress));
        }

        [TestMethod]
        public void TestSetStatusInProgressEmpty()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(string.Empty, ADIU.StatusWorklist.InProgress));
        }


        [TestMethod]
        public void TestSetStatusDiscontinued()
        {
            List<ADIU.Appointment> list = new List<ADIU.Appointment>();
            list.AddRange(iADIU.GetAppointments());
            ADIU.Appointment appointment = list[list.Count - 1];

            string affectedSOP = iADIU.CreateStatusAppointments(appointment);

            Assert.IsTrue(iADIU.SetStatusAppointments(affectedSOP, ADIU.StatusWorklist.Discontinued));
        }

        [TestMethod]
        public void TestSetStatusDiscontinuedNull()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(null, ADIU.StatusWorklist.Discontinued));
        }

        [TestMethod]
        public void TestSetStatusDiscontinuedEmpty()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(string.Empty, ADIU.StatusWorklist.Discontinued));
        }


        [TestMethod]
        public void TestSetStatusAppointCompleted()
        {
            List<ADIU.Appointment> list = new List<ADIU.Appointment>();
            list.AddRange(iADIU.GetAppointments());
            ADIU.Appointment appointment = list[list.Count-1];

            string affectedSOP = iADIU.CreateStatusAppointments(appointment);

            iADIU.SetStatusAppointments(affectedSOP, ADIU.StatusWorklist.InProgress);

            Assert.IsTrue(iADIU.SetStatusAppointments(affectedSOP, ADIU.StatusWorklist.Completed));
        }

        [TestMethod]
        public void TestSetStatusCompletedSOPNull()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(null, ADIU.StatusWorklist.Completed));
        }

        [TestMethod]
        public void TestSetStatusCompletedSOPEmpty()
        {
            Assert.IsFalse(iADIU.SetStatusAppointments(string.Empty, ADIU.StatusWorklist.Completed));
        }

    }
}
