using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;

namespace ADIU
{
    /// <summary>
    /// ADIU Service exposes the features: 
    /// -Retrieve Appointments
    /// -Set Status of Appointements
    /// 
    /// </summary>
    public class ADIUService : IADIU
    {
        MergeHandler handler;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ADIUService(MergeHandler handler):base()
        {
            //Handler has the configuration of the Merge DICOM Client(Port,AE Title)
            this.handler = handler;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ADIUService ()
        {
            //Handler has the configuration of the Merge DICOM Client(Port,AE Title)
            handler = MergeHandler.Instance;

            //Initilize Merge COM Dll
            handler.Initialize();
            //Register the Application Entity into the DICOM Dll
            handler.RegisterApp();
            //Create client configurations to communicate with provider 
            handler.CreateContextList();
        }

        /// <summary>
        /// Get all the Appointments of the Provider.
        /// The WCF only supports arrays not lists
        /// </summary>
        /// <returns>Array of appointments</returns>
        Appointment[] IADIU.GetAppointments()
        {
            try
            {
                List<Appointment> appointments = new List<Appointment>();

                WorkList workList = new WorkList();
                workList.Handler = handler;
                appointments = workList.GetList();

                return appointments.ToArray();
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Send the actual status on the client to the Provider worlist 
        /// </summary>
        /// <param name="affectedSOPInstance">Instance UID of the status state</param>
        /// <param name="status">Status value</param>
        /// <returns>Return true if it was sucessful. Otherwise, returns false</returns>
        bool IADIU.SetStatusAppointments(string affectedSOPInstance, StatusWorklist status)
        {
            try
            {
                bool result = false;
                WorklistProgress progress = new WorklistProgress();
                progress.Handler = handler;
                progress.AffectedSOPInstance = affectedSOPInstance;

                switch (status)
                {
                    case StatusWorklist.Completed:
                        {
                            progress.SendNSETRQComplete();
                            result = true;
                            break;
                        }
                    case StatusWorklist.Discontinued:
                        {
                            progress.SetProgress("DISCONTINUED");
                            result = true;
                            break;
                        }
                    case StatusWorklist.InProgress:
                        {

                            progress.SetProgress("IN PROGRESS");
                            result = true;
                            break;
                        }
                    default:
                        {
                            result = false;
                            break;
                        }
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Create a status register in the provider
        /// </summary>
        /// <param name="appointment">Appointment that have the status</param>
        /// <returns>Return the Instance UID of status register</returns>
        string IADIU.CreateStatusAppointments(Appointment appointment)
        {     
            try
            {
                WorklistProgress progress = new WorklistProgress();
                progress.Handler = handler;
                progress.Appointment = appointment;
                progress.CreateProgress();

                return progress.AffectedSOPInstance;
            }
            catch(Exception)
            {
                return null;
            }
        }  
    }
}
