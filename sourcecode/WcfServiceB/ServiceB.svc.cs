﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceB
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceB : IServiceB
    {
        ServiceReferenceA.ServiceAClient srvA;

        public bool ConnectionOK()
        {
            try
            {
                srvA = new ServiceReferenceA.ServiceAClient();
                srvA.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public List<global::PatientDLL.Patient> getPatients()
        {
            return new List<PatientDLL.Patient> {
                new PatientDLL.Patient("B1", 1),
                new PatientDLL.Patient("B2", 2),
                new PatientDLL.Patient("B3", 3) };
        }
        public List<global::PatientDLL.Patient> getPatientsFromA()
        {
            try
            {
                srvA = new ServiceReferenceA.ServiceAClient();
                List<global::PatientDLL.Patient> patients = srvA.getPatients().ToList() ;
                srvA.Close();

                return patients;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
