using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;

namespace QRSCU
{
    class Program
    {
        static void Main(string[] args)
        {
            
            QuerySCU querySCU = new QuerySCU();
            querySCU.IniFilePath = "C:\\Users\\dcosta\\Documents\\GitHub\\Dummy\\WCFHospitalSim\\src\\ADIU\\QRSCU\\bin\\MERGE.INI";
            querySCU.LicenseNum = "F47D-4E28-F854";
            querySCU.Initialize();
            QueryFields queryFields = new QueryFields(QuerySCU.PATIENT_ROOT_MODEL, QuerySCU.PATIENT_LEVEL);
            queryFields.fields[0].val = "Demo166";
            MCproposedContextList qrContextList = querySCU.CreateContextList();
            ArrayList resultlist = new ArrayList();
            querySCU.RegisterApp();
            querySCU.CreateRemoteApp(qrContextList);
            //Start Query Check
            querySCU.CheckCFINDresults(queryFields, resultlist, qrContextList);




        }
    }
}
