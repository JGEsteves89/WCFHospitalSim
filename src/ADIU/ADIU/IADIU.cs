using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ADIU
{
    [ServiceContract(Namespace = "http://ADIU")]
    public interface IADIU
    {
        [OperationContract]
        Appointment[] GetAppointments();

        [OperationContract]
        bool SetStatusAppointments(string studyid, Status_Worklist status);
    }
}
