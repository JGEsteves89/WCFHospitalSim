using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceA
    {
        [OperationContract]
        List<global::PatientDLL.Patient> getPatients();

        [OperationContract]
        List<global::PatientDLL.Patient> getPatientsFromB();

        // TODO: Add your service operations here
    }

}
