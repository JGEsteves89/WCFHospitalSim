using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ADIU
{
    /// <summary>
    /// Interface for the Acuity User
    /// </summary>
    [ServiceContract(Namespace = "http://ADIU")]
    public interface IADIU
    {
        /// <summary>
        /// Get all the Appointments of the Provider
        /// </summary>
        /// <returns>Array of appointments</returns>
        [OperationContract]
        Appointment[] GetAppointments();

        /// <summary>
        /// Send the actual status on the client to the Provider worlist 
        /// </summary>
        /// <param name="affectedSOPInstance">Instance UID of the status state</param>
        /// <param name="status">Status value</param>
        /// <returns>Return true if it was sucessful. Otherwise, returns false</returns>
        [OperationContract]
        bool SetStatusAppointments(string affectedSOPInstance, StatusWorklist status);

        /// <summary>
        /// Create a status register in the provider
        /// </summary>
        /// <param name="appointment">Appointment that have the status</param>
        /// <returns>Return the Instance UID of status register</returns>
        [OperationContract]
        string CreateStatusAppointments(Appointment appointment);
    }
}
