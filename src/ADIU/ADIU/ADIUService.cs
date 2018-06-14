using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;

namespace ADIU
{
    public class ADIUService : IADIU
    {
        MergeHandler handler;

        public ADIUService ()
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            handler = MergeHandler.Instance;
            handler.RemoteHost = null;
            handler.LicenseNum = System.Configuration.ConfigurationManager.AppSettings["LicenseNum"];
            handler.RemotePort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RemotePort"]);
            handler.RemoteAE = System.Configuration.ConfigurationManager.AppSettings["RemoteAE"];
            handler.LocalAE = System.Configuration.ConfigurationManager.AppSettings["LocalAE"];
            handler.IniFilePath = System.Configuration.ConfigurationManager.AppSettings["IniFilePath"];
            handler.SecureAssociation = false;
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
                List<WorkPatient> workPatients = workList.GetList();
                workPatients.ForEach(r => appointments.Add(ConvertFromWorkPatient(r)));

                return  appointments.ToArray();
            }
            catch(Exception)
            {
                return null;
            }
        }

        Appointment ConvertFromWorkPatient(WorkPatient patient)
        {
            Appointment appointment = new Appointment();
            appointment.Accession = patient.Accession;
            appointment.Modality = patient.Modality;
            appointment.PatientBirthDay = patient.PatientBirthDay;
            appointment.PatientID = patient.PatientID;
            appointment.PatientName = patient.PatientName;
            appointment.PatientSex = patient.PatientSex;
            appointment.PhysicianName = patient.PhysicianName;
            appointment.ProcedureDesc = patient.ProcedureDesc;
            appointment.ProcedureID = patient.ProcedureID;
            appointment.StartDate = patient.StartDate;
            appointment.StartTime = patient.StartTime;
            appointment.StepID = patient.StepID;
            appointment.StudyInstance = patient.StudyInstance;
            return appointment;
        }


        bool IADIU.SetStatusAppointments(string studyid, Status_Worklist status)
        {
            try
            {
                WorklistProgress progress = new WorklistProgress();
                progress.Handler = handler;
                progress.Patient = new WorkPatient();
                progress.Patient.StudyInstance = studyid;
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
    }
}
