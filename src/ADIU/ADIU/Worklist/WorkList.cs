using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;
using System.ComponentModel;

namespace ADIU
{
    public class WorkList:MergeBase
    {

        public WorkList()
        {
            Threshold = 31;
            Timeout = 3000000;
        }
       
      
        #region attributes
      

   
        public String AETitle
        {
            get; set;
        }
        public String Modality
        {
            get; set;
        }
        public String PatientName
        {
            get; set;
        }
       
        public String StartDate
        {
            get; set;
        }
        [DefaultValue(31)]
        public int Threshold 
        {
            get;
            set;
        }
    
        #endregion
     


        public List <WorkPatient> GetList()
        {
            try
            {
                List<WorkPatient> workPatients = new List<WorkPatient>();
                OpenAssociation();
                MCdimseMessage reqMessage = new MCdimseMessage();

                BuildMsg(ref reqMessage);
                sendCFINDMsg(ref reqMessage);
                workPatients = ProcessWorkListReplyMsg(reqMessage);

                CloseAssociation();

                return workPatients;
            }
            catch(Exception et)
            {
                Util.println("Cannot create an association with the remote application");
                throw et;
            }
           
        }
   

        public void BuildMsg(ref MCdimseMessage message)
        {
            MCitem item = null;
            //Create DIMSE message with "Modality C-FIND"
            try
            {
                message = new MCdimseMessage(MCdimseService.C_FIND_RQ, "MODALITY_WORKLIST_FIND");
            }
            catch (MCillegalArgumentException e)
            {
                Util.printError("Unable to create the message to send", e);
                throw e;
            }
            //Create item Object
            item = new MCitem("SCHEDULED_PROCEDURE_STEP");
            //Join DIMSE Message with item to create dataset to send SCP
            try
            {
                message.DataSet[MCdicom.SCHEDULED_PROCEDURE_STEP_SEQUENCE, 0] = item;
            }
            catch (MCexception e1)
            {
                Util.printError("Unable to set the item in message", e1);
                throw e1;
            }
            try
            {
                // set the modality
                if (Modality != null && Modality.Length > 0)
                    item[MCdicom.MODALITY, 0] = Modality;
                else
                    item[MCdicom.MODALITY, 0] = null;

                //set the local application name
                if (AETitle != null && AETitle.Length > 0)
                    item[MCdicom.SCHEDULED_STATION_AE_TITLE, 0] = AETitle;
                else
                    item[MCdicom.SCHEDULED_STATION_AE_TITLE, 0] = null;

                // set the start date
                if (StartDate != null && StartDate.Length > 0)
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_START_DATE, 0] = StartDate;
                else
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_START_DATE, 0] = null;

                // The Scheduled Procedure Step ID tag is used in the second query to
                // guarentee uniqueness.  Set it properly here if supplied.
                item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = null;
            }
            catch (MCexception e1)
            {
                Util.printError("Unable to set the value in message", e1);
                throw e1;
            }
            /*
                * OK.  We are though with setting the required fields with their entered
                * data or a wild card.  Now we must "NULL out" all of the other fields
                * that the server is going to look at.
                */
            try
            {
                /*
                * We don't allow the user to set a value for the time.  Since we don't
                * allow them to enter a time, we give the SCP a universal match for
                * the entire day.
                */
                item[MCdicom.SCHEDULED_PROCEDURE_STEP_START_TIME, 0] = null;

                /*
                * We don't allow query by physician's name, so we will blank it out.
                */
                item[MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME, 0] = null;

                /*
                * Again, if the requested procedure id isn't set, we will set it
                * to the NULL value.
                */
                message.DataSet[MCdicom.REQUESTED_PROCEDURE_ID, 0] = null;

                /*
                * Requested procedure description...
                */
                message.DataSet[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = null;

                /*
                * Again, if the study instance uid isn't set, we will set it
                * to the NULL value.
                */
                message.DataSet[MCdicom.STUDY_INSTANCE_UID, 0] = null;

                /*
                * Again, if the patient name isn't set, we will set it
                * to the NULL value.
                */
                if (PatientName != null && PatientName.Length > 0)
                {
                    message.DataSet[MCdicom.PATIENTS_NAME, 0] = PatientName;
                }
                else
                    message.DataSet[MCdicom.PATIENTS_NAME, 0] = null;

                /*
                * Again, if the patient id isn't set, we will set it
                * to the NULL value.
                */
                message.DataSet[MCdicom.PATIENT_ID, 0] = null;
                message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = null;
                message.DataSet[MCdicom.PATIENTS_SEX, 0] = null;
                message.DataSet[MCdicom.REFERRING_PHYSICIANS_NAME, 0] = null;
                message.DataSet[MCdicom.ACCESSION_NUMBER, 0] = null;
                message.DataSet[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = null;
                message.DataSet[MCdicom.REFERENCED_STUDY_SEQUENCE, 0] = null;
                message.DataSet[MCdicom.REQUESTED_PROCEDURE_CODE_SEQUENCE, 0] = null;
               // message.DataSet[MCdicom.STUDY_ID, 0] = null;//New Study ID

                item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = null;
                item[MCdicom.SCHEDULED_PROTOCOL_CODE_SEQUENCE, 0] = null;
            }
            catch (MCexception e1)
            {
                Util.printError("Unable to set the value in message", e1);
                throw e1;
            }
        }
        public void sendCFINDMsg(ref MCdimseMessage message)
        {
            MCbasicWorklistManagementService service = new MCbasicWorklistManagementService(myAssoc);
            try
            {
                service.sendRequestMessage(message);
            }
            catch (MCexception e2)
            {
                Util.printError("Error sending the WorkList message", e2);
                throw e2;
            }
           
        }
        public List<WorkPatient> ProcessWorkListReplyMsg(MCdimseMessage reqMsg)
        {
            List<WorkPatient> workPatients = new List<WorkPatient>();
            MCdimseMessage message = null; // The response message from the SCP
            ushort response; // The response value from the SCP
            MCitem item = null; // The item number that pertains to the sequence of procedure steps 
            WorkPatient patientRec; // to stock the Patient data
            int messageCount = 0; // the number of responses from SCP
            bool alreadySent = false; // to know if a C_CANCEL_RQ has been sent to the SCP 

            /*
            * Until finished,  we'll loop, looking for messages from the server.
            */
            for (; ; )
            {

                /*
                * Read a message from the formed association, timing out in
                * timeout seconds.
                */
                try
                {
                    message = myAssoc.read(Timeout);
                }
                catch (MCexception e)
                {
                    Util.printError("Error reading the message from SCP", e);
                    throw e;
                }

                if (message == null)
                {
                    Util.println("Timeout while waiting for message from SCP.");
                    throw new TimeoutException();
                }

                /*
                * Once we've read the message, then we check the operation to make
                * sure that we haven't completed the reading...
                */
                try
                {
                    response = message.ResponseStatus;
                }
                catch (MCexception e1)
                {
                    Util.printError("Error reading the status of the message", e1);
                    throw e1;
                }

                if (response == MCdimseService.C_FIND_SUCCESS)
                {
                    /*
                    * Since we've gotten a success, we've read the last message
                    * from the server.
                    */
                    break;
                }
                else if (response == MCdimseService.C_FIND_PENDING_NO_OPTIONAL_KEY_SUPPORT || response == MCdimseService.C_FIND_PENDING)
                {
                    /*
                    * Here is where we actually grab the data from the toolkit's
                    * data strctures and attempt to display it to the user.
                    */
                    patientRec = new WorkPatient();

                    /*
                    * Since some of the responses are contained within a sequence,
                    * we will attempt to open that sequence as an item:
                    */
                    try
                    {
                        item = (MCitem)message.DataSet[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_SEQUENCE), 0];
                    }
                    catch (MCexception e2)
                    {
                        Util.printError("Error attempting to read the scheduled procedure step sequence", e2);
                        throw e2;
                    }

                    /*
                    * If we were given a start date, then fill it in.
                    */
                    try
                    {
                        MCdate date = item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_START_DATE), 0] as MCdate;
                        patientRec.StartDate = date == null ? "" : date.ToString();
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the scheduled procedure step start date", e3);
                        throw e3;
                    }

                    /*
                    * If we were given a start time, then fill it in.
                    */
                    try
                    {
                        MCtime time = item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_START_TIME), 0] as MCtime;
                        patientRec.StartTime = time == null ? "" : time.ToString();
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the scheduled procedure step start time", e3);
                        throw e3;
                    }

                    /*
                    *  When given a modality, fill it in.
                    */
                    try
                    {
                        patientRec.Modality = ((String)item[new MCtag(MCdicom.MODALITY), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the modality", e3);
                        throw e3;
                    }

                    /*
                    * Now we have a physician's name, so fill it in.
                    */
                    try
                    {
                        MCpersonName name = item[new MCtag(MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME), 0] as MCpersonName;
                        patientRec.PhysicianName = name == null ? "" : name.ToString();
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the physician's name", e3);
                        throw e3;
                    }

                    /*
                    * When given a scheduled procedure step, we want to fill
                    * that in as well.
                    */
                    try
                    {
                        patientRec.StepID = ((String)item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_ID), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the scheduled procedure step ID", e3);
                        throw e3;
                    }

                    /*
                    * When given a scheduled procedure step description, we want
                    * to fill that in as well.
                    */
                    try
                    {
                        patientRec.StepDesc = ((String)item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the scheduled procedure step description", e3);
                        throw e3;
                    }

                    /*
                    * If given a requested procedure id, then we read it in.
                    */
                    try
                    {
                        patientRec.ProcedureID = ((String)message.DataSet[new MCtag(MCdicom.REQUESTED_PROCEDURE_ID), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the request procedure ID", e3);
                        throw e3;
                    }

                    /*
                    *  If given a requested procedure description, then we read it in.
                    */
                    try
                    {
                        patientRec.ProcedureDesc = ((String)message.DataSet[new MCtag(MCdicom.REQUESTED_PROCEDURE_DESCRIPTION), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the requested procedure description", e3);
                        throw e3;
                    }

                    /*
                    *  If we have a study instance UID, then fill it in.
                    */
                    try
                    {
                        patientRec.StudyInstance = ((String)message.DataSet[new MCtag(MCdicom.STUDY_INSTANCE_UID), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the study instance UID", e3);
                        throw e3;
                    }

                    /*
                    *  If we have an accession number, then fill it in.
                    */
                    try
                    {
                        patientRec.Accession = ((String)message.DataSet[new MCtag(MCdicom.ACCESSION_NUMBER), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the accession number", e3);
                        throw e3;
                    }

                    /*
                    * Now we grab the patient's name
                    */
                    try
                    {
                        MCpersonName name = message.DataSet[new MCtag(MCdicom.PATIENTS_NAME), 0] as MCpersonName;
                        patientRec.PatientName = name == null ? "" : name.ToString();
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the patient name", e3);
                        throw e3;
                    }

                    /*
                    * And patient ID...
                    */
                    try
                    {
                        patientRec.PatientID = ((String)message.DataSet[new MCtag(MCdicom.PATIENT_ID), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the patient ID", e3);
                        throw e3;
                    }

                    /*
                    *  Patient birth date.
                    */
                    try
                    {
                        MCdate date = message.DataSet[new MCtag(MCdicom.PATIENTS_BIRTH_DATE), 0] as MCdate;
                        patientRec.PatientBirthDay = date == null ? "" : date.ToString();
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the patient birth date", e3);
                        throw e3;
                    }

                    /*
                    *   Patient sex.
                    */
                    try
                    {
                        patientRec.PatientSex = ((String)message.DataSet[new MCtag(MCdicom.PATIENTS_SEX), 0]);
                    }
                    catch (MCexception e3)
                    {
                        Util.printError("Error attempting to read the patient sex", e3);
                        throw e3;
                    }

                    /*
                    * now, we can add the information in the patientMap. 
                    * The key to access a patient in the map is the patient ID. 
                    */
                    workPatients.Add(patientRec);

                    /*
                    * We've read a message.  If we've read more messages than
                    * what we are looking for, then we want to send a
                    * C_CANCEL_RQ back to the SCP.
                    */
                    messageCount++;
                    if (messageCount >= Threshold && alreadySent == false)
                    {
                        /*
                        * We use this variable to make sure that we
                        * only send ONE cancel message to the SCP.
                        */
                        alreadySent = true;

                        MCbasicWorklistManagementService service = new MCbasicWorklistManagementService(myAssoc);
                        try
                        {
                            service.sendCancelRequest(reqMsg);
                        }
                        catch (MCexception e2)
                        {
                            Util.printError("Error sending the Cancel Request message", e2);
                            throw e2;
                        }
                        Util.println("sending C_CANCEL_RQ...");
                    }
                }
                else if (response == MCdimseService.C_FIND_CANCEL_REQUEST_RECEIVED)
                {
                    /*
                    * In this case, the SCP is acknowledging our sending of a
                    * request to cancel the query.
                    */
                    Util.println("Received acknowledgement of a C_CANCEL_RQ message");

                    /*
                    * There are actually two choices.  This code will keep processing
                    * response messages after this SCU has sent out the CANCEL request.
                    * Other designs would need to handle responses, but might not
                    * process the responses until this acknowledgement is received.
                    * In this case, we want to stop looking for responses once we've
                    * received the acknowledgement of the C_CANCEL from the SCP --
                    * hence the "break".
                    */
                    break;
                }
                else
                {
                    /*
                    * OK, now we've gotten an unexpected response from the server.
                    * We can panic now, but we'll attempt to tell the user why...
                    */
                    throw new MCexception(MCexception.INVALID_MESSAGE_RECEIVED, "Obtained an unknown response message from the SCP.");
                } // End of giant if-then-else  

            } // End of infinite loop 
            return workPatients;
        }
    }
}
