﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;

namespace ADIU
{
    /// <summary>
    /// WCF service that implements the Acuity Interface for the user
    /// </summary>
    public class ADIUService : IADIU
    {
        MergeHandler handler;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ADIUService(MergeHandler handler)
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            this.handler = handler;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ADIUService ()
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            handler = MergeHandler.Instance;

            handler.Initialize();
            handler.RegisterApp();
            handler.CreateContextList();
        }
            
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

        bool IADIU.SetStatusAppointments(string affectedSOPInstance, Status_Worklist status)
        {
            try
            {
                WorklistProgress progress = new WorklistProgress();
                progress.Handler = handler;
                progress.AffectedSOPInstance = affectedSOPInstance;

                switch (status)
                {
                    case Status_Worklist.COMPLETED:
                        {
                            progress.SendNSETRQComplete();
                            break;
                        }
                    case Status_Worklist.DISCONTINUED:
                        {
                            progress.SetProgress("DISCONTINUED");
                            break;
                        }
                    case Status_Worklist.IN_PROGRESS:
                        {

                            progress.SetProgress("IN PROGRESS");
                            break;
                        }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

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
