using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;

namespace WorkSCU
{
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            WorkList workList = new WorkList();
            workList.LicenseNum = "F47D-4E28-F854";
            workList.RemoteHost = null;
            workList.RemotePort = 104;
            workList.RemoteAE = "MERGE_WORK_SCP";
            workList.LocalAE = "MERGE_WORK_SCU";
            workList.IniFilePath = "C:\\Users\\dcosta\\Documents\\GitHub\\Dummy\\WCFHospitalSim\\src\\ADIU\\WorkSCU\\bin\\MERGE.INI";
            workList.Initialize();
            workList.RegisterApp();
            workList.CreateContextList();
            List<WorkPatient> workPatients = workList.GetList();
            workPatients.ForEach(r => Console.WriteLine(r.ToString()));
            Console.ReadLine();
        }
        #region oldcode
        public class WorkSCU
        {
            /// <summary>  This function will set all of the necessary attributes for an N_SET
            /// response message./* ================================================================
            

            // Definition of the argument values for the exit() function
            private const int EXIT_SUCCESS = 0;
            private const int EXIT_FAILURE = 1;
            /// </summary>
            /// <exception cref="MCexception">  </exception>
            private static MCdimseMessage NSETRQ
            {
                set
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
                        value.DataSet[MCdicom.PERFORMED_SERIES_SEQUENCE, 0] = performedSeries;
                    }
                    catch (MCexception e)
                    {
                        Util.println("Failed to set the performed series sequence item.");
                        throw e;
                    }
                    try
                    {
                        value.DataSet[MCdicom.REFERENCED_IMAGE_SEQUENCE, 0] = refImage;
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
                        value.RequestedSopInstanceUid = affectedSOPInstance;
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

            }


            


            /* ================================================================
            *  Configuration values
            */
            private static long timeout = 300000; // 5 minutes
            private static String localAE = "MERGE_WORK_SCU";
            private static String remoteAE = "MERGE_WORK_SCP";
            private static int remotePort = 104;
            private static String remoteHost = null;
            private static MCproposedContextList scuContextList = null;
            private static String aeTitle = "";
            private static String modality = "";
            private static String patientName = "";
            private static String startDate = "";
            private static int threshold = 31;
            private static String affectedSOPInstance;
            private static bool secureAssociation = false;

            /* ================================================================
            *  Global variables
            */

            // The list of patients to process
            private static Hashtable patientMap;


            /* ================================================================
            * Class variables
            */

            // Set by main and never changed
            private static MCapplication localApplication = null;

            // Association with the remote application
            private static MCassociation myAssoc = null;


            /// <summary>
            /// This class stock the received information about a patient 
            /// </summary>
            private class WorkPatient
            {

                public String startDate = null;
                public String startTime = null;
                public String modality = null;
                public String stepID = null; // Scheduled procedure step ID
                public String stepDesc = null; // Scheduled procedure step description
                public String physicianName = null;
                public String procedureID = null; // Requested precedure ID
                public String procedureDesc = null; // Requested procedure description
                public String studyInstance = null;
                public String accession = null;
                public String patientName = null;
                public String patientID = null;
                public String patientBirthDay = null;
                public String patientSex = null;

                public WorkPatient() : base()
                {
                }
            }

            /// <summary> ============================================================================
            /// main()
            /// </summary>

            public static void getList()
            {

                MCdimseMessage requestMessage = null;


                // ------------------------------------------------------------------------
                // We must initialize the MergeCOM library first thing
                // ------------------------------------------------------------------------
                FileInfo mergeIniFile = new FileInfo("C:\\Users\\dcosta\\Documents\\GitHub\\Dummy\\WCFHospitalSim\\src\\ADIU\\ADIU\\bin\\MERGE.INI");
                try
                {
                    MC.mcInitialization(mergeIniFile, "F47D-4E28-F854");                    
                }
                catch (MCalreadyInitializedException e)
                {
                    Console.WriteLine(e);
                }
                catch (MCinvalidLicenseInfoError e)
                {
                    Console.WriteLine(e);
                }
                catch (MCruntimeException e)
                {
                    Console.WriteLine(e);
                }

                // ------------------------------------------------------------------------
                // Register this DICOM application
                // ------------------------------------------------------------------------
                try
                {
                    localApplication = MCapplication.getApplication(localAE);
                }
                catch (System.Exception e)
                {
                    Util.printError("Unable to register \"" + localAE + "\".", e);
                    System.Environment.Exit(EXIT_FAILURE);
                }

                // ------------------------------------------------------------------------
                // Create the Proposed Context List (services) we will support
                // ------------------------------------------------------------------------
                createContextList();


                /*
                * We need to create an empty list that will hold our
                * query results.
                */
                patientMap = new Hashtable();

                /*
                * Attempt to open an association with the provider.
                */
                try
                {
                    openAssociation();
                }
                catch (MCexception)
                {
                    throw new System.IO.IOException();
                }

                /*
                * At this point, the user has filled in all of their
                * message parameters, and then decided to query.
                * Therefore, we need to construct and send their message
                * off to the server.
                */
                try
                {
                    requestMessage = setAndSendWorkListMsg(null, null);
                }
                catch (MCexception)
                {
                    /*
                    * Before exiting, we will attempt to close the association
                    * that was successfully formed with the server.
                    */
                    myAssoc.abort();
                    System.Environment.Exit(EXIT_FAILURE);
                }

                /*
                * We've now sent out a CFIND message to a service class
                * provider.  We will now wait for a reply from this server,
                * and then "process" the reply.
                */
                try
                {
                    processWorkListReplyMsg(requestMessage);
                }
                catch (MCexception)
                {
                    myAssoc.abort();
                    System.Environment.Exit(EXIT_FAILURE);
                }

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
                    System.Environment.Exit(EXIT_FAILURE);
                }
                catch (MCoperationNotAllowedException e3)
                {
                    Util.printError("Unable to close the association with the remote application", e3);
                    myAssoc.abort();
                    System.Environment.Exit(EXIT_FAILURE);
                }

                /*
                * If we got no patients back from the server, tell the
                * user that.
                */
                if ((patientMap.Count == 0))
                {
                    Util.println("No patients were found based on the entered criteria.");
                    throw new System.IO.IOException();
                }


            }


            /// <summary> Constructs a proposed context list for WorkSCU
            /// 
            /// </summary>
            private static void createContextList()
            {
                MCproposedContext[] contextArray = new MCproposedContext[2];
                MCtransferSyntaxList tsList = null;
                MCtransferSyntax[] syntaxes = new MCtransferSyntax[3];

                try
                {

                    syntaxes[0] = MCtransferSyntax.ExplicitLittleEndian;
                    syntaxes[1] = MCtransferSyntax.ExplicitBigEndian;
                    syntaxes[2] = MCtransferSyntax.ImplicitLittleEndian;

                    tsList = new MCtransferSyntaxList("SampleQrScpSyntaxes", syntaxes);

                    /*
                    * Create the SCP service list.  This includes the worklist
                    * service list.
                    */
                    contextArray[0] = new MCproposedContext(MCsopClass.getSopClassByName("MODALITY_WORKLIST_FIND"), tsList, true, false);
                    contextArray[1] = new MCproposedContext(MCsopClass.getSopClassByName("PERFORMED_PROCEDURE_STEP"), tsList, true, false);

                    scuContextList = new MCproposedContextList("SampleWorkScuServices", contextArray);
                }
                catch (MCexception e)
                {
                    Util.printError("Unable to create Worklist context", e);
                }
            }

            /// <summary>  Opens an association with the server based on the includance of,
            /// or lack of, command line parameters.
            /// 
            /// </summary>
            /// <exception cref="MCexception"></exception>
            private static void openAssociation()
            {
                MCremoteApplication remApp;
                try
                {
                    if (remoteHost != null)
                        remApp = new MCremoteApplication(remoteAE, new IPEndPoint(Dns.GetHostEntry(remoteHost).AddressList[0], remotePort), scuContextList);
                    else
                        remApp = MCremoteApplication.getObject(remoteAE);

                    if (secureAssociation)
                    {
                        Ssl ssl = new Ssl();

                        ssl.Certificate = "ssl.crt";
                        ssl.Password = "SSL SAMPLE";

                        myAssoc = MCassociation.requestSecureAssociation(localApplication, remApp, ssl);
                    }
                    else
                    {
                        myAssoc = MCassociation.requestAssociation(localApplication, remApp);
                    }
                }
                catch (MCexception e1)
                {
                    Util.println("Cannot create an association with the remote application");
                    throw e1;
                }
            }


            /// <summary>  Opens a message, fills in some of the "required" fields, and then 
            /// sends the message off to the service class provider.
            /// </summary>
            /// <param name="aPatientID">a possibly null patient ID</param>
            /// <param name="aProcedureStepID">The procedure step ID</param>
            /// <returns>  MCdimseMessage requestMessage</returns>
            /// <exception cref="MCexception">when unable to set or send the message </exception>
            private static MCdimseMessage setAndSendWorkListMsg(String aPatientID, String aProcedureStepID)
            {

                MCdimseMessage message = null; //message to send
                MCitem item = null; //Item for the scheduled procedure step

                /*
                * Since we've gotten an association with the remote application
                * (the server), and we know what the user wants to do,  we are able
                * to perform a C-FIND.  But first, we have to setup the message that
                * we'll be passing to the server.  Note:  the WORK_SCP is only
                * capable of a MODALITY_WORKLIST_FIND therefore, our message will be
                * a find.  Setting up the message usually consists of two steps:
                * opening the message, and then setting the message's tag values.
                */


                /*
                * We Open a DIMSE message with C_FIND_RQ command code and 
                * MODALITY_WORKLIST_FIND name. 
                */
                try
                {
                    message = new MCdimseMessage(MCdimseService.C_FIND_RQ, "MODALITY_WORKLIST_FIND");
                }
                catch (MCillegalArgumentException e)
                {
                    Util.printError("Unable to create the message to send", e);
                    throw e;
                }

                /*
                * Some of the values that we want to query on are part of a sequence
                * within the FIND.  Therefore, we attempt to open an item...
                */
                item = new MCitem("SCHEDULED_PROCEDURE_STEP");


                /*
                * Now we have a message and we have an item.  The item is the sequence
                * of procedures.  We need to connect this item to the message.  The
                * following places the item into the message in place of the
                * procedure step sequence.
                */
                try
                {
                    message.DataSet[MCdicom.SCHEDULED_PROCEDURE_STEP_SEQUENCE, 0] = item;
                }
                catch (MCexception e1)
                {
                    Util.printError("Unable to set the item in message", e1);
                    throw e1;
                }

                /*
                * In order to setup the message, we use the setValue or setNullValue functions.
                * We also include some fields that are part of the IHE requirements.
                * We set those values to NULL, indicitating that the SCP should give
                * them to us if it can.
                */
                try
                {
                    // set the modality
                    if (modality != null && modality.Length > 0)
                        item[MCdicom.MODALITY, 0] = modality;
                    else
                        item[MCdicom.MODALITY, 0] = null;

                    //set the local application name
                    if (aeTitle != null && aeTitle.Length > 0)
                        item[MCdicom.SCHEDULED_STATION_AE_TITLE, 0] = aeTitle;
                    else
                        item[MCdicom.SCHEDULED_STATION_AE_TITLE, 0] = null;

                    // set the start date
                    if (startDate != null && startDate.Length > 0)
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_START_DATE, 0] = startDate;
                    else
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_START_DATE, 0] = null;

                    // The Scheduled Procedure Step ID tag is used in the second query to
                    // guarentee uniqueness.  Set it properly here if supplied.
                    if (aProcedureStepID != null && aProcedureStepID.Length > 0)
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = aProcedureStepID;
                    else
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
                    if (patientName != null && patientName.Length > 0)
                    {
                        message.DataSet[MCdicom.PATIENTS_NAME, 0] = patientName;
                    }
                    else
                        message.DataSet[MCdicom.PATIENTS_NAME, 0] = null;

                    /*
                    * Again, if the patient id isn't set, we will set it
                    * to the NULL value.
                    */
                    if (aPatientID != null && aPatientID.Length > 0)
                    {
                        message.DataSet[MCdicom.PATIENT_ID, 0] = aPatientID;
                    }
                    else
                        message.DataSet[MCdicom.PATIENT_ID, 0] = null;

                    message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = null;
                    message.DataSet[MCdicom.PATIENTS_SEX, 0] = null;
                    message.DataSet[MCdicom.REFERRING_PHYSICIANS_NAME, 0] = null;
                    message.DataSet[MCdicom.ACCESSION_NUMBER, 0] = null;
                    message.DataSet[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = null;
                    message.DataSet[MCdicom.REFERENCED_STUDY_SEQUENCE, 0] = null;
                    message.DataSet[MCdicom.REQUESTED_PROCEDURE_CODE_SEQUENCE, 0] = null;

                    item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = null;
                    item[MCdicom.SCHEDULED_PROTOCOL_CODE_SEQUENCE, 0] = null;
                }
                catch (MCexception e1)
                {
                    Util.printError("Unable to set the value in message", e1);
                    throw e1;
                }

                /*
                * Once the message has been built, we are then able to perform the
                * C-FIND on it.
                * To send request messages we use the sendRequestMessage method of
                * the MCbasicWorklistManagementService class.
                */
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

                return message;
            }


            /// <summary> Waits for a reply message(s) from the service class provider.
            /// Once the server sends a message, we then want to process the
            /// message.  In this test program, we process the message by just
            /// printing out the contents of the response.
            /// </summary>
            /// <param name="reqMsg">used for cancel request</param>
            /// <exception cref="MCexception"></exception>
            private static void processWorkListReplyMsg(MCdimseMessage reqMsg)
            {

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
                        message = myAssoc.read(timeout);
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
                            patientRec.startDate = date == null ? "" : date.ToString();
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
                            patientRec.startTime = time == null ? "" : time.ToString();
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
                            patientRec.modality = ((String)item[new MCtag(MCdicom.MODALITY), 0]);
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
                            patientRec.physicianName = name == null ? "" : name.ToString();
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
                            patientRec.stepID = ((String)item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_ID), 0]);
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
                            patientRec.stepDesc = ((String)item[new MCtag(MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION), 0]);
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
                            patientRec.procedureID = ((String)message.DataSet[new MCtag(MCdicom.REQUESTED_PROCEDURE_ID), 0]);
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
                            patientRec.procedureDesc = ((String)message.DataSet[new MCtag(MCdicom.REQUESTED_PROCEDURE_DESCRIPTION), 0]);
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
                            patientRec.studyInstance = ((String)message.DataSet[new MCtag(MCdicom.STUDY_INSTANCE_UID), 0]);
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
                            patientRec.accession = ((String)message.DataSet[new MCtag(MCdicom.ACCESSION_NUMBER), 0]);
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
                            patientRec.patientName = name == null ? "" : name.ToString();
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
                            patientRec.patientID = ((String)message.DataSet[new MCtag(MCdicom.PATIENT_ID), 0]);
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
                            patientRec.patientBirthDay = date == null ? "" : date.ToString();
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
                            patientRec.patientSex = ((String)message.DataSet[new MCtag(MCdicom.PATIENTS_SEX), 0]);
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
                        patientMap[patientRec.patientID] = patientRec;

                        /*
                        * We've read a message.  If we've read more messages than
                        * what we are looking for, then we want to send a
                        * C_CANCEL_RQ back to the SCP.
                        */
                        messageCount++;
                        if (messageCount >= threshold && alreadySent == false)
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
            }

           

            /// <summary>  This is the second sub-menu, that takes the list of patients that was
            /// obtained earlier, and then asks the user to select one.  After selecting
            /// a specific patient, another C_FIND_REQUEST is performed.  More detailed
            /// information is displayed to the user.
            /// It is a simple addition to this sub-menu to implement performed
            /// procedure step, once it gains approval.
            /// 
            /// </summary>
            /// <exception cref="MCexception"></exception>
            private static void gatherMoreData()
            {

                System.IO.StreamReader stdin;
                MCdimseMessage requestMessage = null;
                bool goodNumber = false;
                IEnumerator patientIter; // Iterator to read the patients in the Map
                String[] arrayPatientID;
                WorkPatient patient = null; // selected patient
                String aPatientID;
                String aPatientName;
                int numPatient;
                String aLine;
                String spaces = "                              "; // 30 spaces
                String readLine = null;

                /*
                * Display the second menu with patient names an patient ID
                */
                do
                {
                    Util.println("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Util.println("              Patient Name                 Patient ID");
                    Util.println("              ------------                 ----------");


                    /*
                    * the Iterator skim through the set of patient ID,
                    * then, we can find the patient name. 
                    * The ID are stored in a String array to associate number
                    * with each of them. 
                    */
                    /* We also need to sort the keys to the patient map so that
                     * the list is displayed in a predictable manner (the enumerator
                     * implementation seem to differ between .NET 2.0 and 4.5. See
                     * COM-1413)
                     */
                    ICollection keys = patientMap.Keys;
                    Array sortedKeys = Array.CreateInstance(typeof(String), keys.Count);
                    keys.CopyTo(sortedKeys, 0);
                    Array.Sort(sortedKeys);
                    patientIter = sortedKeys.GetEnumerator();
                    arrayPatientID = new String[patientMap.Count];
                    numPatient = 0;
                    while (patientIter.MoveNext())
                    {
                        aPatientID = ((String)patientIter.Current);
                        aPatientName = ((WorkPatient)patientMap[aPatientID]).patientName;
                        arrayPatientID[numPatient] = aPatientID;
                        numPatient++;
                        aLine = "[" + numPatient + "]" + spaces.Substring(6 + aPatientName.Length) + aPatientName + spaces.Substring(4 + aPatientID.Length) + aPatientID;
                        Util.println(aLine);
                    }
                    Util.println("Select a patient to start a Modality Performed Procedure Step for, ");
                    Util.println("or 'x' to exit: ");
                    Util.println("");
                    Util.println("==> ");

                    readLine = Util.Line;

                    goodNumber = true;
                    try
                    {
                        if (readLine != null && readLine[0] != 'x' && readLine[0] != 'X')
                        {
                            /*
                            * Get the patient information associated with the number that
                            * they entered.
                            */
                            try
                            {
                                patient = (WorkPatient)patientMap[arrayPatientID[System.Int32.Parse(readLine) - 1]];
                            }
                            catch (System.FormatException)
                            {
                                throw new System.IO.IOException();
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                                throw new System.IO.IOException();
                            }
                        }
                        else if (readLine != null && (readLine[0] == 'x' || readLine[0] == 'X'))
                        {
                            return;
                        }
                        else
                        {
                            throw new System.IO.IOException();
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        goodNumber = false;
                        Util.println(readLine + " is invalid");
                        Util.println("Press <return> to continue.");
                        stdin = new System.IO.StreamReader(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).CurrentEncoding);
                        try
                        {
                            stdin.ReadLine();
                        }
                        catch (System.IO.IOException)
                        {
                            Util.println("IO Error reading from console");
                            System.Environment.Exit(EXIT_FAILURE);
                        }
                    }
                }
                while (goodNumber == false);


                /*
                * Attempt to open an association with the provider.
                */
                try
                {
                    openAssociation();
                }
                catch (MCexception e1)
                {
                    throw e1;
                }

                /*
                * Create and send another C-FIND message.
                */
                try
                {
                    requestMessage = setAndSendWorkListMsg(patient.patientID, patient.stepID);
                }
                catch (MCexception e2)
                {
                    /*
                    * Before exiting, we will attempt to close the association
                    * that was successfully formed with the server.
                    */
                    myAssoc.abort();
                    throw e2;
                }

                /*
                * Wait for, and process another C-FIND reply message.
                */
                patientMap = new Hashtable();
                try
                {
                    processWorkListReplyMsg(requestMessage);
                }
                catch (MCexception e4)
                {
                    myAssoc.abort();
                    throw e4;
                }

                /*
                * Now that we are done with the SCP, we'll close the association that
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

                /*
                * We must look to see how many responses we got back from the worklist
                * SCP.  We should only get one.  Should we get none, then the patient
                * whom we've obtained via the initial worklist query is no longer
                * available to us (perhaps another modality is already working on that
                * patient).
                */
                if (patientMap.Count == 0)
                {
                    Util.println("\nUnable to obtain patient ID: " + patient.patientID + " from the worklist SCP.");
                    Util.println("Perhaps the patient is no longer available.");
                    Util.println("Please try again.\n");
                }
                else if (patientMap.Count > 1)
                {
                    /*
                    * This sample application can only handle a single matching
                    * patient based on the initial query criteria.  Of course,
                    * a real application MUST implement this possibility, and NOT
                    * with an error message like is being done here!!!!!
                    */
                    Util.println("\nFound more that one patient matching patient ID: " + patient.patientID + " from the worklist SCP.\n");
                    Util.println("This sample application can only handle a single reply.");
                    Util.println("You must requery from the initial query options.\n");
                }
                else
                {
                    /*
                    * At this point, we go into the Modality Performed Procedure Step
                    * functionality.
                    * We consider this choice to be the start of the MPPS.  That is why
                    * you can exit from this menu without choosing a patient.   
                    */
                    performMPPS();
                }

                Util.println("Press <return> to continue.");
                String ln = Console.ReadLine();

            }


            /// <summary>  This is the third sub-menu.  After identifying a patient via Modality
            /// worklist, and then verifying that a patient actually exists, this
            /// function sends the initial N_CREATE message to the procedure step SCP,
            /// and then gives the user the ability to do some modality imaging
            /// functionality.
            /// </summary>
            private static void performMPPS()
            {
                WorkPatient patient; //Scheduled patient
                bool goodNumber = false;
                bool acqStarted = false;
                bool acqCompleted = false;

                String spaces = "                              "; // 30 spaces
                String readLine = null;
                int choice = 0;

                /*
                * Another sub-menu...  This time, we simulate an actual modality
                * in which we do image acquision and relay performed procedure step
                * information to an SCP.
                */
                IEnumerator e = patientMap.Keys.GetEnumerator();
                e.MoveNext();
                String key = (String)e.Current;
                patient = (WorkPatient)patientMap[key];
                do
                {
                    Util.println("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Util.println("Scheduled patient:");

                    Util.println("       Patient Name                 Patient ID");
                    Util.println(spaces.Substring(10 + patient.patientName.Length) + patient.patientName + spaces.Substring(4 + patient.patientID.Length) + patient.patientID);
                    Util.println("");
                    Util.println("           Modality          Procedure Step ID");
                    Util.println(spaces.Substring(11 + patient.modality.Length) + patient.modality + spaces.Substring(3 + patient.stepID.Length) + patient.stepID);
                    Util.println("");
                    Util.println("    Physicians name               Procedure ID");
                    Util.println(spaces.Substring(11 + patient.physicianName.Length) + patient.physicianName + spaces.Substring(3 + patient.procedureID.Length) + patient.procedureID);
                    Util.println("");
                    Util.println("     Study Instance");
                    Util.println("     " + patient.studyInstance);
                    Util.println("");
                    Util.println("** MODALITY PERFORMED PROCEDURE STEP **");
                    Util.println("");
                    Util.println("[1] Start image acquisition (N_CREATE MPPS)");
                    Util.println("[2] Image w/o complete      (N_SET MPPS w/ image info)");
                    Util.println("[3] Complete and exit MPPS  (N_SET MPPS status=COMPLETED)");
                    Util.println("[X] Exit, without starting MPPS");
                    Util.println("==> ");

                    readLine = Util.Line;

                    try
                    {
                        if (readLine[0] == 'x' || readLine[0] == 'X')
                        {
                            /* 
                            * Before break'ing, we need to verify if acquisition was started
                            * but not completed.
                            */
                            if (acqStarted == true && acqCompleted == false)
                            {
                                Util.println("");
                                Util.println("You cannot start a MPPS instance without");
                                Util.println("finishing it.  Please complete the image");
                                Util.println("acquisition before attempting to exit this");
                                Util.println("sub-menu.\n");
                                throw new System.IO.IOException();
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            try
                            {
                                choice = System.Int32.Parse(readLine);
                            }
                            catch (System.FormatException)
                            {
                                Util.println(readLine + "is invalid");
                                throw new System.IO.IOException();
                            }
                            if (choice < 1 || choice > 3)
                            {
                                Util.println(readLine + " is invalid");
                                throw new System.IO.IOException();
                            }
                            else
                            {
                                /*
                                * They entered a "good" number.
                                */
                                goodNumber = true;

                                /*
                                * Based on what the user entered, we send out modality performed
                                * precedure step messages.
                                */
                                switch (choice)
                                {

                                    case 1:

                                        /*
                                        * The user chose to simulate image acquisition.
                                        */
                                        if (acqStarted == false)
                                        {
                                            Util.println("Starting image acquisition...");
                                            acqStarted = true;
                                            try
                                            {
                                                sendNCREATERQ(patient);
                                            }
                                            catch (MCexception)
                                            {
                                                Util.println("Failed to successfully send an NCREATE ");
                                                Util.println("request message to the SCP and receive");
                                                Util.println("a valid response back from the SCP.  ");
                                                Util.println("Cannot start image acquisition.");
                                                acqStarted = false;
                                            }
                                        }
                                        else
                                        {
                                            Util.println("Since starting image acquisition on the above");
                                            Util.println("patient has already been accomplished, you");
                                            Util.println("cannot perform this function again on the");
                                            Util.println("same patient.");
                                        }
                                        throw new System.IO.IOException();


                                    case 2:

                                        /*
                                        * The user chose to update patient information.
                                        */
                                        if (acqStarted == true)
                                        {
                                            Util.println("Sending N_SET RQ w/o status...");
                                            try
                                            {
                                                sendNSETRQ();
                                            }
                                            catch (MCexception)
                                            {
                                            }
                                        }
                                        else
                                        {
                                            Util.println("Cannot update patient without first starting image acquisition.");
                                        }
                                        throw new System.IO.IOException();


                                    case 3:

                                        /*
                                        * The user chose to complete this procedure step.
                                        */
                                        if (acqStarted == true)
                                        {
                                            Util.println("Sending N_SET RQ status=COMPLETED...");
                                            try
                                            {
                                                sendNSETRQComplete();
                                            }
                                            catch (MCexception)
                                            {
                                            }
                                            acqCompleted = true;
                                        }
                                        else
                                        {
                                            Util.println("You cannot complete image acquisition without");
                                            Util.println("having first started it.");
                                            throw new System.IO.IOException();
                                        }
                                        break;
                                } // end of switch
                            }
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        goodNumber = false;
                        Util.println("Press <return> to continue.");
                        String ln = Console.ReadLine();
                    }
                }
                while (goodNumber == false);

            }


            /// <summary> This function simulates the acquision of an image by an imaging
            /// modality.  As a consequence of that performed procedure, information
            /// regarding a performed procedure step is relayed to the worklist/mpps
            /// </summary>
            /// <param name="patient">the scheduled patient
            /// 
            /// </param>
            /// <exception cref="MCexception"></exception>
            private static void sendNCREATERQ(WorkPatient patient)
            {

                MCdimseMessage sendMessage;
                MCdimseMessage responseMessage;
                ushort response;
                String mppsStatus = null;


                /*
                * In order to send the PERFORMED PROCEDURE STEP N_CREATE message to the
                * SCP, we need to open a message.
                */
                try
                {
                    sendMessage = new MCdimseMessage(MCdimseService.N_CREATE_RQ, "PERFORMED_PROCEDURE_STEP");
                }
                catch (MCillegalArgumentException e)
                {
                    Util.println("Unable to open a message for PERFORMED_PROCEDURE_STEP");
                    throw e;
                }

                /*
                * Once we have an open message, we will then attempt to fill in the
                * contents of the message.  This is done in a separate function.
                */
                try
                {
                    setNCREATERQ(sendMessage, patient);
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
                    openAssociation();
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
                    responseMessage = myAssoc.read(timeout);
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
                    affectedSOPInstance = responseMessage.AffectedSopInstanceUid;
                }
                catch (MCexception e)
                {
                    Util.println("Failed to read the affected SOP instance UID in the N_CREATE_RSP message from the SCP.");
                    myAssoc.abort();
                    throw e;
                }
                Util.println("Received affected SOP instance UID: " + affectedSOPInstance + " in N_CREATE response message.");


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
            private static void setNCREATERQ(MCdimseMessage message, WorkPatient patient)
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
                    item[MCdicom.STUDY_INSTANCE_UID, 0] = patient.studyInstance;
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
                    message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_ID, 0] = patient.procedureID;
                }
                catch (MCexception e1)
                {
                    throw e1;
                }
                try
                {
                    message.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_DESCRIPTION, 0] = patient.procedureDesc;
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
                    message.DataSet[MCdicom.PERFORMED_STATION_AE_TITLE, 0] = localAE;
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
                    message.DataSet[MCdicom.MODALITY, 0] = patient.modality;
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
                    if (patient.patientID == null || (System.Object)patient.patientID == (System.Object)"")
                        message.DataSet[MCdicom.PATIENT_ID, 0] = null;
                    else
                        message.DataSet[MCdicom.PATIENT_ID, 0] = patient.patientID;

                    if (patient.patientName == null || (System.Object)patient.patientName == (System.Object)"")
                        message.DataSet[MCdicom.PATIENTS_NAME, 0] = null;
                    else
                        message.DataSet[MCdicom.PATIENTS_NAME, 0] = patient.patientName;

                    if (patient.patientSex == null || (System.Object)patient.patientSex == (System.Object)"")
                        message.DataSet[MCdicom.PATIENTS_SEX, 0] = null;
                    else
                        message.DataSet[MCdicom.PATIENTS_SEX, 0] = patient.patientSex;

                    if (patient.procedureID == null || (System.Object)patient.procedureID == (System.Object)"")
                        item[MCdicom.REQUESTED_PROCEDURE_ID, 0] = null;
                    else
                        item[MCdicom.REQUESTED_PROCEDURE_ID, 0] = patient.procedureID;

                    if (patient.procedureDesc == null || (System.Object)patient.procedureDesc == (System.Object)"")
                        item[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = null;
                    else
                        item[MCdicom.REQUESTED_PROCEDURE_DESCRIPTION, 0] = patient.procedureDesc;

                    if (patient.stepID == null || (System.Object)patient.stepID == (System.Object)"")
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = null;
                    else
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_ID, 0] = patient.stepID;

                    if (patient.stepDesc == null || (System.Object)patient.stepDesc == (System.Object)"")
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = null;
                    else
                        item[MCdicom.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, 0] = patient.stepDesc;

                    if (patient.accession == null || (System.Object)patient.accession == (System.Object)"")
                        item[MCdicom.ACCESSION_NUMBER, 0] = null;
                    else
                        item[MCdicom.ACCESSION_NUMBER, 0] = patient.accession;

                    if (patient.physicianName == null || (System.Object)patient.physicianName == (System.Object)"")
                        item[MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME, 0] = null;
                    else
                        item[MCdicom.SCHEDULED_PERFORMING_PHYSICIANS_NAME, 0] = patient.physicianName;

                    if (patient.patientBirthDay == null || (System.Object)patient.patientBirthDay == (System.Object)"")
                        message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = null;
                    else
                        message.DataSet[MCdicom.PATIENTS_BIRTH_DATE, 0] = patient.patientBirthDay;
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


            /// <summary>  This function will send an N_SET request message to the MPPS SCP.
            /// The N_SET message will contain a performed series and the associated
            /// images.  The status of the performed procedure step isn't updated.
            /// To complete the MPPS instance, another function will be used.
            /// </summary>
            /// <exception cref="MCexception"></exception>
            private static void sendNSETRQ()
            {

                MCdimseMessage sendMessage, responseMessage;
                String mppsStatus = null;

                Util.println("Attempting to N_SET a MPPS with a requested SOP instance UID of:");
                Util.println(affectedSOPInstance);

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
                    NSETRQ = sendMessage;
                }
                catch (MCexception e)
                {
                    /*
                    * No need for an error message, since SetNSETRQ will log one for us
                    * as to why it failed.
                    */
                    throw e;
                }

                /*
                * After we've successfully create the N_SET request message, we can
                * attempt to open the association with the MPPS SCP.
                */
                try
                {
                    openAssociation();
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
                    responseMessage = myAssoc.read(timeout);
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
                catch (MCexception)
                {
                }
                if (mppsStatus != null)
                    Util.println("Received performed procedure step status: " + mppsStatus + " in N_SET response message.");


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


            /// <summary> This function updates the status of a modality performed procedure
            /// step to reflect the fact that this imaging procedure is completed
            /// at this simulated modality.
            /// </summary>
            /// <exception cref="MCexception"></exception>
            private static void sendNSETRQComplete()
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
                    sendMessage.RequestedSopInstanceUid = affectedSOPInstance;
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
                    sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_DATE, 0] =
                        DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
                    sendMessage.DataSet[MCdicom.PERFORMED_PROCEDURE_STEP_END_TIME, 0] =
                        DateTime.Now.ToLocalTime().ToString("HHmmss");
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
                    openAssociation();
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
                    responseMessage = myAssoc.read(timeout);
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
                catch (MCexception)
                {
                }

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

            /// <summary>  Generate a performed procedure instance when there is no worklist
            /// management entry available.
            /// </summary>
            private static void handleStatProc()
            {
                WorkPatient patient = new WorkPatient();

                Util.println("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Util.println(" ** HANDLE STAT PROCEDURE **\n");
                Util.println("In order to process a STAT procedure, a");
                Util.println("procedure ID will need to be entered.");
                Util.println("Please enter a Procedure ID for this patient:");
                Util.println("==>");

                patient.procedureID = Util.Line;

                Util.println("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Util.println(" ** HANDLE STAT PROCEDURE **\n");
                Util.println("In order to process a STAT procedure, a");
                Util.println("modality will need to be entered.");
                Util.println("Please enter a modality for this patient:");
                Util.println("==>");

                patient.modality = Util.Line;

                patient.startDate = "";
                patient.startTime = "";
                patient.stepID = "";
                patient.physicianName = "";
                patient.procedureDesc = "";
                patient.studyInstance = Util.createInstanceUid(); /* create a UID */
                patient.accession = "";
                patient.patientName = "";
                patient.patientID = "";
                patient.patientBirthDay = "";
                patient.patientSex = "";

                patientMap = new Hashtable();
                patientMap[patient.patientID] = patient;
                performMPPS();
            }


            /// <summary> GetOptions is the routine that parses the command line and sets the
            /// global variables associated with each parameter.
            /// 
            /// </summary>
            /// <param name="args">
            /// </param>
            /// <returns> true if all went well
            /// </returns>
            private static bool getOptions(String[] args)
            {


                if (args.Length < 1)
                {
                    /*
                    * They didn't enter the remote app name, so give them a usage, then
                    * return FAILURE.
                    */
                    Util.println("Usage: work_scu [options] remote_ae_title");
                    Util.println("       work_scu -h\n");
                    return false;
                }

                for (int i = 0; i < args.Length; i++)
                {

                    if (args[i][0] == '-')
                    {
                        try
                        {
                            if (args[i].ToUpper() == "-SSL")
                            {
                                secureAssociation = true;
                                continue;
                            }

                            switch (args[i][1])
                            {
                                case 'h':
                                case 'H':  /* help */
                                    Util.println("");
                                    Util.println("Modality Worklist Service Class User\n");
                                    Util.println("Usage: work_scu [options] remote_ae_title\n");
                                    Util.println("remote_ae_title The remote application entity to connect to");
                                    Util.println("Options :");
                                    Util.println("\t -h                 Print this help page");
                                    Util.println("\t -a <ae_title>      Local application entity");
                                    Util.println("\t -n <host name>     Remote Host Name");
                                    Util.println("\t -p <number>        Remote Host Port Number");
                                    Util.println("\t -ssl               Use secure DICOM connection");
                                    Util.println("\t -t <number>        Timeout value");
                                    Util.println("");
                                    return false;

                                case 'a':
                                case 'A':  /* Application Title */
                                    try
                                    {
                                        localAE = args[++i];
                                    }
                                    catch (System.IndexOutOfRangeException e)
                                    {
                                        throw e;
                                    }
                                    Util.println("'-a' received.  Setting local application title to: " + localAE);
                                    break;


                                case 'n':
                                case 'N':  /* remote Host */
                                    try
                                    {
                                        remoteHost = args[++i];
                                    }
                                    catch (System.IndexOutOfRangeException e)
                                    {
                                        throw e;
                                    }
                                    Util.println("'-n' received.  Setting remote host name to: " + remoteHost);
                                    break;


                                case 'p':
                                case 'P':  /* port Number */
                                    try
                                    {
                                        remotePort = System.Int32.Parse(args[++i]);
                                    }
                                    catch (System.IndexOutOfRangeException e)
                                    {
                                        throw e;
                                    }
                                    catch (System.FormatException e)
                                    {
                                        throw e;
                                    }
                                    Util.println("'-p' received.  Setting remote port to: " + remotePort);
                                    break;


                                case 't':
                                case 'T':  /* time out */
                                    try
                                    {
                                        timeout = System.Int32.Parse(args[++i]);
                                    }
                                    catch (System.IndexOutOfRangeException e)
                                    {
                                        throw e;
                                    }
                                    catch (System.FormatException e)
                                    {
                                        throw e;
                                    }
                                    Util.println("'-t' received.  Setting timeout to: " + timeout);
                                    break;
                                case '0':
                                    i++;
                                    if (args[i][0] == '-')
                                    {
                                        Util.println("Missing value for " + args[--i] + " option.");
                                        return false;
                                    }
                                    try
                                    {
                                        Console.SetIn(new System.IO.StreamReader(args[i]));
                                    }
                                    catch (System.ArgumentNullException)
                                    {
                                        Util.println("Ignoring invalid input file: " + args[i]);
                                    }
                                    break;


                                default:
                                    throw new System.Exception();

                            }
                        }
                        catch (System.Exception)
                        {
                            Util.println("\nUsage: work_scu [options] remote_ae_title");
                            Util.println("       work_scu -h\n");
                            return false;
                        }
                    }
                    else
                    {
                        /* This must be the remote application title */
                        try
                        {
                            remoteAE = args[i];
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            Util.println("\nUsage: work_scu [options] remote_ae_title");
                            Util.println("       work_scu -h\n");
                            return false;
                        }
                    }
                } // end for i

                if (remoteAE == null || remoteAE.Length == 0)
                {

                    /* Application Title is a required parameter */
                    Util.println("Required parameter 'Remote Application Entity Title' not specified.");
                    Util.println("Usage: work_scu [options] remote_ae_title\n");
                    return false;
                }

                if (remoteHost != null || remotePort > 0)
                {
                    if (remoteHost == null || remoteHost.Length > 0 && remotePort == -1)
                    {
                        Util.println("Remote host name and port number should be specified together.");
                        return false;
                    }
                }


                return true;
            }
        }
        #endregion
    }
}
