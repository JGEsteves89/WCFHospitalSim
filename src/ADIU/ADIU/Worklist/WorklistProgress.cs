using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;
using System.ComponentModel;


namespace ADIU
{
    public class WorklistProgress:MergeBase
    {

        public WorkPatient Patient
        {
            get;
            set;
        }

        public String AffectedSOPInstance
        {
            get;
            set;
        }

        #region Create Progress

        /// <summary> This function simulates the acquision of an image by an imaging
        /// modality.  As a consequence of that performed procedure, information
        /// regarding a performed procedure step is relayed to the worklist/mpps
        /// </summary>
        /// <param name="patient">the scheduled patient
        /// 
        /// </param>
        /// <exception cref="MCexception"></exception>
        public void CreateProgress()
        {

            MCdimseMessage sendMessage;
            MCdimseMessage responseMessage;
            ushort response;
            String mppsStatus = null;


            /* In order to send the PERFORMED PROCEDURE STEP N_CREATE message to the
            * SCP, we need to open a message. */
            try
            {
                sendMessage = new MCdimseMessage(MCdimseService.N_CREATE_RQ, "PERFORMED_PROCEDURE_STEP");
            }
            catch (MCillegalArgumentException e)
            {
                Util.println("Unable to open a message for PERFORMED_PROCEDURE_STEP");
                throw e;
            }

            /* Once we have an open message, we will then attempt to fill in the
            * contents of the message.  This is done in a separate function.*/
            try
            {
                setNCREATERQ(ref sendMessage);
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Now that we've created our request message, we want to open the 
            * association with an SCP that is providing MPPS.
            */
            try
            {
                OpenAssociation();
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * And finally, we send the N_CREATE message to the server.
            */
            MCdimseService service = new MCstudyManagementService(myAssoc);
            try
            {
                service.sendRequestMessage(sendMessage);
            }
            catch (MCexception e2)
            {
                Util.printError("Unable to send the request message to the SCP.", e2);
                throw e2;
            }

            /*
            * We must examine the N_SET response message, especially looking for
            * the affected SOP instance UID.  This UID will be used to track the
            * other N_SET functionality later.
            */
            try
            {
                responseMessage = myAssoc.read(Timeout);
            }
            catch (MCexception e3)
            {
                Util.println("Unable to read the response message from the SCP.");
                throw e3;
            }

            if (responseMessage == null)
            {
                Util.println("Timeout while waiting for response message from the SCP.");
                throw new TimeoutException();
            }

            /*
            * And then check the status of the response message.
            * It had better be N_SET_SUCCESS or else we will exit.  A real application
            * will probably choose something better than just exiting, though.
            */
            try
            {
                response = responseMessage.ResponseStatus;
            }
            catch (MCexception e1)
            {
                Util.printError("Error reading the status of the message", e1);
                throw e1;
            }
            if (response != MCdimseService.N_CREATE_SUCCESS)
            {
                Util.println("Unexpected response status from the SCP.");
                myAssoc.abort();
                throw new MCinvalidMessageReceivedException("Unexpected response status from the SCP.");
            }

            /*
            * Note that we only look for the affected SOP instance UID and STATUS in
            * the response message.  In reality, a MPPS SCU will probably want to
            * examine other attributes that are in the response message.  The SCP
            * will send back an attribute for everyone that was asked for in the
            * request/create message.
            */
            try
            {
                AffectedSOPInstance = responseMessage.AffectedSopInstanceUid;
            }
            catch (MCexception e)
            {
                Util.println("Failed to read the affected SOP instance UID in the N_CREATE_RSP message from the SCP.");
                myAssoc.abort();
                throw e;
            }
            Util.println("Received affected SOP instance UID: " + AffectedSOPInstance + " in N_CREATE response message.");


            /*
            * Although our SCP returns the contents of a message that has been 
            * set, there is no guanrentee the SCP will do this, so this is not
            * a failure condition.
            */
            try
            {
                mppsStatus = ((String)responseMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_STATUS, 0]);
            }
            catch (MCexception)
            {
            }
            if (mppsStatus != null)
                Util.println("Received performed procedure step status: " + mppsStatus + " in N_CREATE response message.");

            /*
            * Now that we are done, we'll close the association that
            * we've formed with the server.
            */
            try
            {
                myAssoc.release();
            }
            catch (MCassociationAbortedException e3)
            {
                Util.printError("Unable to close the association with the remote application", e3);
                throw e3;
            }
            catch (MCoperationNotAllowedException e3)
            {
                Util.printError("Unable to close the association with the remote application", e3);
                myAssoc.abort();
                throw e3;
            }
        }

        /// <summary> This function will set all of the N_CREATE attributes for a message
        /// that is passed in and return it.  The message must be valid.
        /// 
        /// </summary>
        /// <param name="message">a valid message</param>
        /// <param name="patient">the scheduled patient
        /// </param>
        /// <exception cref="MCexception"></exception>
        private void setNCREATERQ(ref MCdimseMessage message)
        {

            MCitem item;
            String sentUID;

            /*
            * Here, we setup all of the needed pieces to the performed procedure
            * step N_CREATE message.  These fields that are listed first are all of
            * the required fields.  After these, we will fill in some type 2 fields
            * that tie the whole Modality Worklist and Performed Procedure Step
            * stuff together.
            *
            *
            * First, we need an item to hold some of the attributes.
            */
            item = new MCitem("SCHEDULED_STEP_ATTRIBUTE");

            /*
            * Then we place the reference to this item into the message itself.
            */
            try
            {
                message.DataSet[MCdicom.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE, 0] = item;
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the ITEM in message.");
                throw e;
            }

            /*
            * and then, start filling in some of the attributes that are part of
            * the item.
            * First part of the item:  study instance UID.
            */
            try
            {
                item[MCdicom.STUDY_INSTANCE_UID, 0] = Patient.StudyInstance;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Performed procedure step ID
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_ID, 0] = Patient.ProcedureID;
            }
            catch (MCexception e1)
            {
                throw e1;
            }
            try
            {
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_DESCRIPTION, 0] = Patient.ProcedureDesc;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Performed station AE title
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_STATION_AE_TITLE, 0] = Handler.LocalAE;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Performed procedure step start date
            * Both the data and time should be obtained right here.  This happens
            * because the worklist only contains the scheduled start date and time,
            * and not the actual start date and time.
            */
            try
            {
                String date = System.DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_START_DATE, 0] =
                    date;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Performed procedure step start time
            */
            try
            {
                String time = System.DateTime.Now.ToLocalTime().ToString("HHmmss");
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_START_TIME, 0] =
                    time;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Performed procedure step status
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_STATUS, 0] = "IN PROGRESS";
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Modality
            */
            try
            {
                message.DataSet[MCdicom.MODALITY, 0] = Patient.Modality;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Requested SOP instance UID
            * This is required by IHE, but not required in general.  If an SCU
            * doesn't send this value, then the SCP will create the UID and send it
            * back to us.
            * In this case, since we sent it, the SCP should send the same UID back
            * to us.
            */
            sentUID = Util.createInstanceUid("1.2.840.10008.3.1.2.3.3");
            Util.println("Sending requested SOP instance UID: " + sentUID);
            try
            {
                message.AffectedSopInstanceUid = sentUID;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * OK, now we are done with the required fields.  The next few fields
            * will be used by the MPPS SCP to tie this MPPS N_CREATE request to
            * a particular patient, and all of that other worklist stuff.
            * These fields aren't required by a MPPS SCP.  They are type 2.  In cases
            * where MPPS is used with Modality Worklist, it makes sense to send the
            * key patient demographic information that was just obtained via the
            * worklist.  In fact, since our sample worklist SCP is also the MPPS SCP,
            * the SCP requires these fields in order to "tie" the information together.
            * If this information isn't sent, then the SCP will create a MPPS instance
            * that is not tied to any particular patient, and will update your
            * patient demographic information upon receipt of an N_SET.
            */

            /*
            * First, set the attributes we retrieved from the worklist, that we 
            * should have values for.
            */
            try
            {
                if (Patient.PatientID == null || (System.Object)Patient.PatientID == (System.Object)"")
                    message.DataSet[MCdicom.PATIENT_ID, 0] = null;
                else
                    message.DataSet[MCdicom.PATIENT_ID, 0] = Patient.PatientID;

                if (Patient.PatientName == null || (System.Object)Patient.PatientName == (System.Object)"")
                    message.DataSet[MCdicom.PATIENTS_NAME, 0] = null;
                else
                    message.DataSet[MCdicom.PATIENTS_NAME, 0] = Patient.PatientName;

                if (Patient.PatientSex == null || (System.Object)Patient.PatientSex == (System.Object)"")
                    message.DataSet[MCdicom.PATIENTS_SEX, 0] = null;
                else
                    message.DataSet[MCdicom.PATIENTS_SEX, 0] = Patient.PatientSex;

                if (Patient.ProcedureID == null || (System.Object)Patient.ProcedureID == (System.Object)"")
                    item[MCdicom.REQUESTED_PROCEDURE_ID, 0] = null;
                else
                    item[MCdicom.REQUESTED_PROCEDURE_ID, 0] = Patient.ProcedureID;

                if (Patient.ProcedureDesc == null || (System.Object)Patient.ProcedureDesc == (System.Object)"")
                    item[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = null;
                else
                    item[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = Patient.ProcedureDesc;

                if (Patient.StepID == null || (System.Object)Patient.StepID == (System.Object)"")
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = null;
                else
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = Patient.StepID;

                if (Patient.StepDesc == null || (System.Object)Patient.StepDesc == (System.Object)"")
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = null;
                else
                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = Patient.StepDesc;

                if (Patient.Accession == null || (System.Object)Patient.Accession == (System.Object)"")
                    item[MCdicom.ACCESSION_NUMBER, 0] = null;
                else
                    item[MCdicom.ACCESSION_NUMBER, 0] = Patient.Accession;

                if (Patient.PhysicianName == null || (System.Object)Patient.PhysicianName == (System.Object)"")
                    item[MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME, 0] = null;
                else
                    item[MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME, 0] = Patient.PhysicianName;

                if (Patient.PatientBirthDay == null || (System.Object)Patient.PatientBirthDay == (System.Object)"")
                    message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = null;
                else
                    message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = Patient.PatientBirthDay;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Now set the attributes that we do not have from the worklist, but
            * are type 2, so we decide to send these due to the IHE requirements.
            * Use the SetNullValue function
            * If you have a value for these, simply use the setValue function.
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_STATION_NAME, 0] = null;
                message.DataSet[MCdicom.PERFORMED_LOCATION, 0] = null;
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_DESCRIPTION, 0] = null;
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_TYPE_DESCRIPTION, 0] = null;
                message.DataSet[MCdicom.STUDY_ID, 0] = null;
            }
            catch (MCexception e1)
            {
                throw e1;
            }


            /*
            * Now, set several tags that will be set in the N-SET message, but we 
            * don't know yet.
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_DATE, 0] = null;
                message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_TIME, 0] = null;
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * Now, set several sequence attributes to NULL that are required,
            * but we don't support.  Note that we're still using the
            * SetNullValue function.  We probably should have written a
            * seperate function that just does sequences.
            */
            try
            {
                item[MCdicom.REFERENCED_STUDY_SEQUENCE, 0] = null;
                item[MCdicom.SCHEDULED_PROTOCOL_CODE_SEQUENCE, 0] = null;
                message.DataSet[MCdicom.REFERENCED_PATIENT_SEQUENCE, 0] = null;
                message.DataSet[MCdicom.PROCEDURE_CODE_SEQUENCE, 0] = null;
                message.DataSet[MCdicom.PERFORMED_PROTOCOL_CODE_SEQUENCE, 0] = null;
                message.DataSet[MCdicom.PERFORMED_SERIES_SEQUENCE, 0] = null;
            }
            catch (MCexception e1)
            {
                throw e1;
            }
        }

        #endregion

        #region Set Progress

        /// <summary>  This function will send an N_SET request message to the MPPS SCP.
        /// The N_SET message will contain a performed series and the associated
        /// images.  The status of the performed procedure step isn't updated.
        /// To complete the MPPS instance, another function will be used.
        /// </summary>
        /// <exception cref="MCexception"></exception>
        public void SetProgress(string progress)
        {

            MCdimseMessage sendMessage, responseMessage;
            String mppsStatus = null;

            Util.println("Attempting to N_SET a MPPS with a requested SOP instance UID of:");
            Util.println(AffectedSOPInstance);

            /*
            * This function simulates the actual imaging process of the sample
            * modality that this program simulates.  In this case, we attempt to
            * open the N_SET request message, and let another function take care of
            * actually building the message, itself.
            */
            try
            {
                sendMessage = new MCdimseMessage(MCdimseService.N_SET_RQ, "PERFORMED_PROCEDURE_STEP");
            }
            catch (MCillegalArgumentException e)
            {
                Util.println("Unable to open a N_SET request message.");
                throw e;
            }

            /*
            * Then, we attempt to create the actual N_SET request message.
            */
            try
            {
                ProcesNSETRQ(ref sendMessage);
            }
            catch (MCexception e)
            {
                /*
                * No need for an error message, since SetNSETRQ will log one for us
                * as to why it failed.
                */
                throw e;
            }

            /* And finally, we set the status to completed.*/
            try
            {
                sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_STATUS, 0] = progress;
            }
            catch (MCexception e)
            {
                Util.println("Unable to set the performed procedure step status.");
                throw e;
            }

            /*
            * After we've successfully create the N_SET request message, we can
            * attempt to open the association with the MPPS SCP.
            */
            try
            {
                OpenAssociation();
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * And finally, we send the request message to the server.
            */
            MCdimseService service = new MCstudyManagementService(myAssoc);
            try
            {
                service.sendRequestMessage(sendMessage);
            }
            catch (MCexception e2)
            {
                Util.printError("Unable to send the request message to the SCP.", e2);
                throw e2;
            }

            /*
            * We must examine the N_SET response message, especially looking for
            * the affected SOP instance UID.  This UID will be used to track the
            * other N_SET functionality later.
            */
            try
            {
                responseMessage = myAssoc.read(Timeout);
            }
            catch (MCexception e3)
            {
                Util.println("Unable to read the response message from the SCP.");
                throw e3;
            }

            if (responseMessage == null)
            {
                Util.println("Timeout while waiting for response message from the SCP.");
                throw new TimeoutException();
            }

            /*
            * Although our SCP sends the status in the response, there is no 
            * guarentee it will be there
            */
            try
            {
                mppsStatus = ((String)responseMessage.DataSet[new MCtag(MCdicom.PERFORMED_PROCEDURE_STEP_STATUS), 0]);
            }
            catch (MCexception ex)
            {
                CloseAssociation();
                throw ex;
            }
            if (mppsStatus != null)
                Util.println("Received performed procedure step status: " + mppsStatus + " in N_SET response message.");


            /*
            * Now that we are done, we'll close the association that
            * we've formed with the server.
            */
            CloseAssociation();
        }

        void ProcesNSETRQ(ref MCdimseMessage message)
        {
            MCitem performedSeries, refImage;

            /*
            * Now we need some items, that are part of the main message.
            */
            performedSeries = new MCitem("PERFORMED_SERIES");
            refImage = new MCitem("REF_IMAGE");

            /*
            * And place reference to the sequence item in its respective place.
            */
            try
            {
                message.DataSet[MCdicom.PERFORMED_SERIES_SEQUENCE, 0] = performedSeries;
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the performed series sequence item.");
                throw e;
            }
            try
            {
                message.DataSet[MCdicom.REFERENCED_IMAGE_SEQUENCE, 0] = refImage;
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the referenced image sequence item.");
                throw e;
            }
    
            /*
            * OK, we now have the message and the sequences setup, so we now need to
            * start placing attributes in the appropriate places.
            */

            /*
            * First, we place the requested SOP instance UID into the request message.
            * This will tell the SCP exactly which MPPS instance we are asking it to
            * perform the N_SET upon.
            */
            try
            {
                message.RequestedSopInstanceUid = AffectedSOPInstance;
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the SOP instance UID");
                throw e;
            }

            /*
            * Since we are adding an image record, we need to add all of the tags
            * that are needed for the performed series sequence item.
            * The information contained within these tags is being derived at the
            * point of the imaging function.  That is, we are creating these values
            * RIGHT NOW!
            */
            try
            {
                performedSeries[MCdicom.PERFORMING_PHYSICIANS_NAME, 0] = "DR^PHYSICIAN";
                performedSeries[MCdicom.PROTOCOL_NAME, 0] = "PROTOCOLE";
                performedSeries[MCdicom.OPERATORS_NAME, 0] = "DR^PHYSICIAN";
                performedSeries[MCdicom.SERIES_INSTANCE_UID, 0] = Util.createInstanceUid();
                performedSeries[MCdicom.SERIES_DESCRIPTION, 0] = "TESTSERIES";
                performedSeries[MCdicom.RETRIEVE_AE_TITLE, 0] = "RETR_AE_TITLE";
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the tags for performed series sequence item.");
                throw e;
            }

            /*
            * Now, we're ready for the referenced SOP class UID.  In real life, this
            * is the SOP class UID that the modality is attempting to USE.  We will
            * fill it in with the UID for CT images.  This will need to change in an
            * actual implementation.
            */
            try
            {
                refImage[MCdicom.REFERENCED_SOP_CLASS_UID, 0] = "1.2.840.10008.5.1.4.1.1.2";
                refImage[MCdicom.REFERENCED_SOP_INSTANCE_UID, 0] = Util.createInstanceUid();
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the referenced SOP class UID");
                throw e;
            }
            try
            {
                // Setting value to null explicitly
                refImage[MCdicom.REFERENCED_STANDALONE_SOP_INSTANCE_SEQUENCE, 0] = null;
            }
            catch (MCexception e)
            {
                Util.println("Failed to set the referenced standalone SOP instance sequence to NULL.");
                throw e;
            }
        }

        #endregion

        /// <summary> This function updates the status of a modality performed procedure
        /// step to reflect the fact that this imaging procedure is completed
        /// at this simulated modality.
        /// </summary>
        /// <exception cref="MCexception"></exception>
        public void SendNSETRQComplete()
        {

            MCdimseMessage sendMessage, responseMessage;
            String mppsStatus = null;

            /*
            * In order to send the PERFORMED PROCEDURE STEP N_SET message to the
            * SCP, we need to open a message.
            */
            try
            {
                sendMessage = new MCdimseMessage(MCdimseService.N_SET_RQ, "PERFORMED_PROCEDURE_STEP");
            }
            catch (MCillegalArgumentException e)
            {
                Util.println("Unable to open a message for PERFORMED_PROCEDURE_STEP N_SET operation");
                throw e;
            }

            /*
            ** Now, we setup all of the needed pieces to the performed procedure
            ** step N_SET message.
            */

            /* 
            ** A group zero element:  requested SOP instance UID.  This UID will have
            ** been returned from the SCP during the N_CREATE call by this SCU to the
            ** SCP.  It is basically a key, created by the SCP that is used by the
            ** SCP to know which data set that the SCU is referencing during the N_SET.
            */
            try
            {
                sendMessage.RequestedSopInstanceUid = AffectedSOPInstance;
            }
            catch (MCexception e)
            {
                Util.println("Unable to set the resquested SOP instance UID.");
                throw e;
            }

            /*
            ** Performed procedure step end date
            ** Both the date and time are obtained at this point because this signifies
            ** the actual end of the procedure step.
            */
            try
            {
                sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_DATE, 0] = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
                sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_TIME, 0] = DateTime.Now.ToLocalTime().ToString("HHmmss");
            }
            catch (MCexception e)
            {
                Util.println("Unable to set the date and time.");
                throw e;
            }

            /* 
            * And finally, we set the status to completed.
            */
            try
            {
                sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_STATUS, 0] = "COMPLETED";
            }
            catch (MCexception e)
            {
                Util.println("Unable to set the performed procedure step status.");
                throw e;
            }


            /*
            * Then, we need to open an association with a modality performed
            * procedure step SCP.
            */
            try
            {
                OpenAssociation();
            }
            catch (MCexception e1)
            {
                throw e1;
            }

            /*
            * And finally, we send the N_SET message to the server.
            */
            MCdimseService service = new MCstudyManagementService(myAssoc);
            try
            {
                service.sendRequestMessage(sendMessage);
            }
            catch (MCexception e2)
            {
                Util.printError("Unable to send the request message to the SCP.", e2);
                throw e2;
            }

            /*
            * We can examine the N_SET response message, verifying that the status
            * comes back as expected.
            */
            try
            {
                responseMessage = myAssoc.read(Timeout);
            }
            catch (MCexception e3)
            {
                Util.println("Unable to read the response message from the SCP.");
                throw e3;
            }

            if (responseMessage == null)
            {
                Util.println("Timeout while waiting for response message from the SCP.");
                throw new TimeoutException();
            }

            /*
            * Although our SCP sends the status in the response, there is no 
            * guarentee it will be there
            */
            try
            {
                mppsStatus = ((String)responseMessage.DataSet[new MCtag(MCdicom.PERFORMED_PROCEDURE_STEP_STATUS), 0]);
                Util.println("Received performed procedure step status: " + mppsStatus + " in N_SET response message.");
            }
            catch (MCexception ex)
            {
                CloseAssociation();
                throw ex;
            }

            CloseAssociation();
        }

    }
}
