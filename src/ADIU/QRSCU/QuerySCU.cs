using System;
using System.Collections;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;

namespace QRSCU
{
    public class QuerySCU
    {
        #region Attributes
        // Constant IDs for Query/Retrieve Models
        public const int PATIENT_ROOT_MODEL = 0;
        public const int STUDY_ROOT_MODEL = 1;
        public const int PATIENT_STUDY_ONLY_MODEL = 2;
        public const int COMPOSITE_MODEL = 3;

        // Constant IDs for Query/Retrieve Levels
        public const int PATIENT_LEVEL = 0;
        public const int STUDY_LEVEL = 1;
        public const int SERIES_LEVEL = 2;
        public const int IMAGE_LEVEL = 3;
        public const int FRAME_LEVEL = 4;

        // We won't service more than this many association at a time
        public const int MAX_THREAD_SERVERS = 5;

        //  Configuration values
        public long timeout { get; set; } = 300000;
        public String applicationTitle { get; set; } = "MERGE_QR_SCU";
        public String storSCPserviceList { get; set; } = "Storage_SCP_Service_List"; // Talvez remover?
        public int maxQueryResponses { get; set; } = 100;
        public int listenPort { get; set; } = -1; //Talvez remover?
        public String remoteApplicationTitle { get; set; } = "MERGE_QR_SCP";
        public String remoteHostname { get; set; } = "127.0.0.1";        
        private static int remotePort = 1004;        
        public String moveDestinationTitle { get; set; } = "KPServer";
        public Boolean secureAssociation { get; set; } = false;
        public String IniFilePath { get; set; }
        public String LicenseNum { get; set; }
        public MCremoteApplication remoteApp = null;    
        public bool examPresent = false;
        #endregion
        /// <summary>
        /// Creates Proposed context list
        /// </summary>
        /// <returns></returns>
        public MCproposedContextList CreateContextList()
        {
            MCproposedContextList list = null;
            MCproposedContext[] contexts = new MCproposedContext[7];
            MCtransferSyntaxList tsList = null;
            MCtransferSyntax[] syntaxes = new MCtransferSyntax[3];
            MCsopClass sop1 = null;
            MCsopClass sop2 = null;
            MCsopClass sop3 = null;
            MCsopClass sop4 = null;
            MCsopClass sop5 = null;
            MCsopClass sop6 = null;
            MCsopClass sop7 = null;

            try
            {
                syntaxes[0] = MCtransferSyntax.ExplicitLittleEndian;
                syntaxes[1] = MCtransferSyntax.ExplicitBigEndian;
                syntaxes[2] = MCtransferSyntax.ImplicitLittleEndian;
                tsList = new MCtransferSyntaxList("SampleQrScpSyntaxes", syntaxes);

                sop1 = MCsopClass.getSopClassByName("PATIENT_STUDY_ONLY_QR_FIND");
                sop2 = MCsopClass.getSopClassByName("PATIENT_STUDY_ONLY_QR_MOVE");
                sop3 = MCsopClass.getSopClassByName("STUDY_ROOT_QR_FIND");
                sop4 = MCsopClass.getSopClassByName("STUDY_ROOT_QR_MOVE");
                sop5 = MCsopClass.getSopClassByName("PATIENT_ROOT_QR_FIND");
                sop6 = MCsopClass.getSopClassByName("PATIENT_ROOT_QR_MOVE");
                sop7 = MCsopClass.getSopClassByName("COMPOSITE_INSTANCE_ROOT_RET_MOVE");

                contexts[0] = new MCproposedContext(sop1, tsList);
                contexts[1] = new MCproposedContext(sop2, tsList);
                // this is an example how to set extended negotiation info
                MCnegotiationInfo negInfo = new MCqueryRetrieveNegotiation(true);
                contexts[2] = new MCproposedContext(sop3, tsList, negInfo);
                contexts[3] = new MCproposedContext(sop4, tsList);
                contexts[4] = new MCproposedContext(sop5, tsList);
                contexts[5] = new MCproposedContext(sop6, tsList);
                contexts[6] = new MCproposedContext(sop7, tsList);

                list = new MCproposedContextList("SampleQrScpServices", contexts);
            }
            catch (MCexception e)
            {
                Util.printError("Unable to create qr context", e);
            }

            return list;
        }
        /// <summary>
        /// Initialize DICOM Library
        /// </summary>
        public void Initialize()
        {
            // ------------------------------------------------------------------------
            // We must initialize the MergeCOM library first thing
            // ------------------------------------------------------------------------
            FileInfo mergeIniFile = new FileInfo(IniFilePath);
            try
            {
                MC.mcInitialization(mergeIniFile, LicenseNum);
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
        }
        // Query/Retrieve Context List
        //   We support all three models of Query/Retrieve
        // This is built at startup time by the createContextList method
        public static MCproposedContextList qrContextList = null;
        /// <summary>
        /// Create local application
        /// </summary>
        private static MCapplication application = null;
        /// <summary>
        /// Register Local App with library
        /// </summary>
        public void RegisterApp()
        {
            // Register this DICOM application
            try
            {
                application = MCapplication.getApplication(applicationTitle);
            }
            catch (Exception e)
            {
                Util.printError("Unable to register \"" + applicationTitle + "\".", e);                
            }
        }
        /// <summary>
        /// Create Remote app object
        /// </summary>
        /// <param name="qrContextList"></param>
        public void CreateRemoteApp(MCproposedContextList qrContextList)
        {
            // set remote application
            try
            {
                if (remoteHostname != null)
                {
                    remoteApp = new MCremoteApplication(remoteApplicationTitle, new IPEndPoint(Dns.GetHostAddresses(remoteHostname)[0], remotePort), qrContextList);
                }
                else
                {
                    remoteApp = MCremoteApplication.getObject(remoteApplicationTitle);
                }
            }
            catch (Exception e)
            {
                Util.printError("Unable to create remote application", e);
            }

        }
        /// <summary> Let user select one of the results of the Query.
        /// </summary>
        /// <param name="list">An ArrayList of ResultFields objects.
        /// </param>
        /// <returns>The ResultFields object containing the selection.
        /// </returns>
        public ResultFields SelectResult(IList list)
        {
            return (ResultFields)list[0];
        }
        /// <summary> 
        /// Opens an association with the SCP and process the C-FIND request.
        /// </summary>
        /// <param name="queryFields">The fields used for the C-FIND request.
        /// </param>
        /// <param name="list">An empty ArrayList to receive the query results.
        /// </param>
        /// <returns>true if all goes well.
        /// </returns>
        public bool HandleCFINDrequest(QueryFields queryFields, ArrayList list)
        {
            bool wentWell = true;
            MCassociation assoc = null;

            // Open as association with SCP 
            try
            {
                if (secureAssociation)
                {
                    Ssl ssl = new Ssl();

                    ssl.Certificate = "ssl.crt";
                    ssl.Password = "SSL SAMPLE";

                    assoc = MCassociation.requestSecureAssociation(application, remoteApp, ssl);
                }
                else
                {
                    assoc = MCassociation.requestAssociation(application, remoteApp);
                }
            }
            catch (Exception e)
            {
                Util.printError("handleCFINDrequest: Unable to open association with \"" + remoteApplicationTitle + "\"", e);
                return false;
            }

            Util.println("Sending the C-FIND request.");

            // Send the request message to the SCP
            wentWell = SendCFINDmessage(queryFields, list, assoc);
            if (!wentWell) Util.println("handleCFINDrequest: sendCFINDmessage returned error, continuing.");

            try
            {
                assoc.release();
            }
            catch (MCexception e)
            {
                Util.printError("Unable to release association", e);
            }

            return wentWell;
        }
        /// <summary> Send a C-FIND request message to the SCP and wait till
        /// all pending responses are received.  The response values
        /// are placed in ResultFields objects in the list array.
        /// </summary>
        /// <param name="queryFields">The fields used for the query.
        /// </param>
        /// <param name="list">An empty Array list to receive responses.
        /// </param>
        /// <param name="assoc">An opened association with the SCP.
        /// </param>
        /// <returns>true if all goes well.
        /// </returns>
        public bool SendCFINDmessage(QueryFields queryFields, ArrayList list, MCassociation assoc)
        {
            ResultFields result = null;
            MCqueryRetrieveService service = null;
            bool done = false;
            bool cancelRequestNotSentYet = true;

            // We will be using the Query/Retrieve Service class
            service = new MCqueryRetrieveService(assoc);

            // Acquire a data set for our query identifier
            MCdataSet ds = null;
            String model;
            try
            {
                // For the Composite Instance models use PATIENT_MODEL_QR_FIND query to find 
                // the required data to retrieve.
                if (queryFields.model == COMPOSITE_MODEL)
                    model = "PATIENT_ROOT";
                else
                    model = queryFields.modelName;

                ds = new MCdataSet(MCdimseService.C_FIND_RQ, model + "_QR_FIND");
            }
            catch (Exception e)
            {
                Util.printError("Unable to construct data set", e);
                return false;
            }

            // Build the query identifier from our query fields
            if (!buildCFINDidentifier(ds, queryFields))
            {
                return false;
            }

            // Send off the message 
            MCdimseMessage requestMsg = null;
            try
            {
                requestMsg = service.sendFindRequest(ds);
            }
            catch (Exception e)
            {
                Util.printError("sendFindRequest() failed", e);
                return false;
            }

            //A single response message is sent for each
            //match to the find request.  Wait for all of these
            //response messages here.  This loop is exited when
            //the status contained in a response message
            //is equal to a failure, or C_FIND_SUCCESS.
            while (!done)
            {
                MCdimseMessage responseMsg = null;

                // Read response message from SCP
                try
                {
                    responseMsg = assoc.read(timeout);

                    if (responseMsg == null)
                    {
                        Util.println("Read timed out - trying again.");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Util.printError("sendCFINDmessage: Error on network read", e);
                    return false;
                }

                // Retrieve and examine the response status
                ushort response;
                try
                {
                    response = responseMsg.ResponseStatus;
                }
                catch (Exception e)
                {
                    Util.printError("sendCFINDmessage: Error on network read", e);
                    return false;
                }

                switch (response)
                {
                    case MCdimseService.C_FIND_SUCCESS:
                        Util.println("Find Response is C_FIND_SUCCESS");
                        done = true;
                        break;

                    case MCdimseService.C_FIND_CANCEL_REQUEST_RECEIVED:
                        //If we get a reply to our cancel, we are finished
                        Util.println("Find Response is C_FIND_CANCEL_REQUEST_RECEIVED");
                        done = true;
                        break;

                    case MCdimseService.C_FIND_PENDING_NO_OPTIONAL_KEY_SUPPORT:
                        Util.println("C_FIND_PENDING_NO_OPTIONAL_KEY_SUPPORT response received.");
                        // Fall through
                        goto case MCdimseService.C_FIND_PENDING;

                    case MCdimseService.C_FIND_PENDING:
                        // Construct an object to receive the response field values
                        // (The constructor copies the queryFields info into the new object)
                        result = new ResultFields(queryFields);

                        if (!HandleCFINDresponse(result, responseMsg))
                        {
                            return false;
                        }

                        // Add the response to our list
                        if (list.Count < maxQueryResponses)
                        {
                            list.Add(result);
                        }
                        else
                        {
                            // We have received too many responses, so we send
                            // a C-CANCEL request to the SCP.  Note that because
                            // there is a delay before the SCP gets the cancel
                            // request, several more responses may continue to
                            // come in.
                            if (cancelRequestNotSentYet)
                            {
                                cancelRequestNotSentYet = false;
                                try
                                {
                                    // Cancel the C-FIND request
                                    service.sendCancelRequest(requestMsg);
                                }
                                catch (Exception e)
                                {
                                    Util.printError("ERROR: send cancel request", e);
                                }
                            }
                        }
                        break;

                    default:
                        Util.println("ERROR: Unknown response to C-FIND request: " + response);
                        done = true;
                        break;

                }
            } // while not done - loop back to read another request message

            return true;
        }
        /// <summary> Get values for all of the attributes requested by our query.  In addition,
        /// get the Retrieve AE Title or fileset information so we know how to
        /// retrieve the image object.  All values are put in the result object.
        /// </summary>
        /// <param name="result">Retrieved values for each field are placed in this object.
        /// </param>
        /// <param name="msg">Values are retrieved from this message's data set.
        /// </param>
        /// <returns>true if all went well.
        /// </returns>
        public bool HandleCFINDresponse(ResultFields result, MCdimseMessage msg)
        {
            // Pending responses contain a dataset 
            // containing the reply identifier attributes.
            MCdataSet ds = msg.DataSet;

            // All pending response message must contain either the Retrieve AE Title
            // (if image object is online) or the File Set ID and UID (if it's offline).
            try
            {
                result.retrieveAEtitle = (String)ds[MCdicom.RETRIEVE_AE_TITLE, 0];
            }
            catch (MCexception e)
            {
                if (e.exceptionNumber == MCexception.NO_SUCH_VALUE || e.exceptionNumber == MCexception.ATTRIBUTE_NOT_FOUND)
                {
                    // No Retrieve AE title, so there must be file information
                    try
                    {
                        result.fileSetId = (String)ds[MCdicom.STORAGE_MEDIA_FILE_SET_ID, 0];
                        result.fileSetUid = (String)ds[MCdicom.STORAGE_MEDIA_FILE_SET_UID, 0];
                    }
                    catch (Exception x)
                    {
                        Util.printError("ERROR: Unable to get image object retrieval information", x);
                        return false;
                    }
                }
                else
                {
                    Util.printError("ERROR: getting MCtagConstants_Fields.RETRIEVE_AE_TITLE", e);
                    return false;
                }
            }

            // Since our query sent all of the keys in its identifier, and we only
            // used Required or Unique Keys, the SCP is required to send back values
            // for all the attributes.
            for (int i = 0; i < result.fields.Length; i++)
            {
                try
                {
                    Object obj;
                    obj = ds[new MCtag(result.fields[i].tag), 0];
                    if (obj == null)
                        result.fields[i].val = null;
                    else if (obj is MCpersonName)
                        result.fields[i].val = ((MCpersonName)obj).ToString();
                    else if (obj is String)
                        result.fields[i].val = (String)obj;
                    else if (obj is MCtime)
                        result.fields[i].val = ((MCtime)obj).ToString();
                    else if (obj is MCdate)
                        result.fields[i].val = ((MCdate)obj).ToString();
                    else if (obj is MCage)
                        result.fields[i].val = ((MCage)obj).ToString();
                    else
                        result.fields[i].val = "";
                }
                catch (MCattributeNotFoundException)
                {
                    result.fields[i].val = "";
                }
                catch (Exception e)
                {
                    Util.printError("ERROR: Unable to get value for " + result.fields[i].name, e);
                    return false;
                }
            }

            return true;
        }
        /// <summary> 
        /// Builds a query identifier in the data set using the fields provided.
        /// NOTE: This method presumes that the data set provided(ds) has no
        /// values yet, and has been constructed for the proper QR service,
        /// C-FIND-RQ command.
        /// </summary>
        /// <param name="ds">A data set constructed for the C-FIND-RQ.
        /// </param>
        /// <param name="queryFields">The fields involved in the query.
        /// </param>
        /// <returns>true if all goes well.
        /// </returns>
        public bool buildCFINDidentifier(MCdataSet ds, QueryFields queryFields)
        {
            // All C-FIND requests must set the query level attribute
            try
            {
                ds[MCdicom.QUERY_RETRIEVE_LEVEL, 0] = queryFields.levelName;
            }
            catch (Exception e)
            {
                Util.printError("buildCFINDmessage: Unable to set value for QR Level", e);
                return false;
            }

            Util.println("We have a " + queryFields.modelName + " Model, " + queryFields.levelName + " Level Query.");

            // We will request that all of the fields be sent back, by setting
            // the identifier attribute to NULL if the user provided no value.
            // Note, however, that the SCP is required to send back the Unique
            // keys for all levels above our query level.  For example, if we
            // are making a "SERIES" level query, the SCP must return at least
            // the Patient ID, Study Instance UID and the Series Instance Uid.
            for (int i = 0; i < queryFields.fields.Length; i++)
            {
                try
                {
                    if (queryFields.fields[i].val.Length == 0) ds[queryFields.fields[i].tag].setValue(null);
                    else ds[queryFields.fields[i].tag].setValue(queryFields.fields[i].val);
                }
                catch (Exception e)
                {
                    Util.printError("Unable to set value for |" + queryFields.fields[i].name + "|", e);
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// This checks if the SCP has the study and returns a bool
        /// </summary>
        /// <param name="queryFields"></param>
        /// <param name="resultlist"></param>
        /// <returns>True if studies are found.</returns>
        public bool CheckCFINDresults (QueryFields queryFields, ArrayList resultlist, MCproposedContextList qrContextList)
        {
            HandleCFINDrequest(queryFields, resultlist);
            if (resultlist.Count > 0)
            {
                Console.WriteLine("Found {0} studies! We can now initiate the C-MOVE command", resultlist.Count);
                examPresent = true;
                ResultFields result = SelectResult(resultlist);
                HandleCMOVErequest(result, qrContextList);
                return true;
            }
            else
            {
                Console.WriteLine("This query did not return results on the remote database!!");
                examPresent = false;
                return false;
            }
        }
        /// <summary> Opens an association with the specified QR SCP and sends a C-MOVE-RQ message.
        /// </summary>
        /// <param name="result">Contains the fields needed to send the C-MOVE-RQ message.
        /// </param>
        /// <returns>true if all went well.
        /// </returns>
        public bool HandleCMOVErequest(ResultFields result, MCproposedContextList qrContextList)
        {
            MCremoteApplication retrieveApp = null;

            // The image object must be online
            if (result.retrieveAEtitle == null || result.retrieveAEtitle.Length == 0)
            {
                Util.println("The location of the image was not given by the archive");
                Util.println("so it is either on offline storage, or its location is not known.");
                return false;
            }

            // Open as association with the Storage SCP 
            MCassociation assoc = null;
            
            try
            {
                if (remoteHostname == null) retrieveApp = MCremoteApplication.getObject(result.retrieveAEtitle);
                else retrieveApp = new MCremoteApplication(result.retrieveAEtitle, new IPEndPoint(Dns.GetHostAddresses(remoteHostname)[0], remotePort), qrContextList);
            }
            catch (Exception e)
            {
                Util.printError("Unable to get MCremoteApplication object", e);
            }

            try
            {
                if (secureAssociation)
                {
                    Ssl ssl = new Ssl();

                    ssl.Certificate = "ssl.crt";
                    ssl.Password = "SSL SAMPLE";

                    assoc = MCassociation.requestSecureAssociation(application, retrieveApp, ssl);
                }
                else
                {
                    assoc = MCassociation.requestAssociation(application, retrieveApp);
                }
            }
            catch (Exception e)
            {
                Util.printError("handleCMOVErequest: Unable to open association with \"" + result.retrieveAEtitle + "\"", e);
                return false;
            }

            Util.println("Sending the C-MOVE request.");

            if (!SendCMOVEmessage(result, assoc))
            {
                Util.println("handleCMOVErequest: sendCMOVEmessage returned error, continuing.");
            }

            // Release the association	
            try
            {
                assoc.release();
            }
            catch (MCexception e)
            {
                Util.printError("Unable to release association", e);
            }

            return true;
        }
        /// <summary> 
        /// Builds a move identifier in the data set using the fields provided.
        /// NOTE: This method presumes that the data set provided(ds) has no
        /// values yet, and has been constructed for the proper QR service,
        /// C-MOVE-RQ command.
        /// </summary>
        /// <param name="ds">A data set constructed for the C-MOVE-RQ.
        /// </param>
        /// <param name="result">Contains the unique keys needed for the move request.
        /// </param>
        /// <returns>true if all goes well.
        /// </returns>
        public bool BuildCMOVEidentifier(MCdataSet ds, ResultFields result)
        {
            // All C-FIND request must set the query level attribute
            try
            {
                ds[MCdicom.QUERY_RETRIEVE_LEVEL, 0] = result.levelName;
            }
            catch (Exception e)
            {
                Util.printError("buildCMOVErequest: Unable to set value for QR Level", e);
                return false;
            }

            try
            {
                switch (result.level)
                {

                    case FRAME_LEVEL:
                        // We only request one image at a time
                        ds[MCdicom.SOP_INSTANCE_UID, 0] = result.getValue(MCdicom.SOP_INSTANCE_UID);

                        string[] frameList = result.getValue(MCdicom.SIMPLE_FRAME_LIST).Split(new char[] { ' ', ',', '.' });
                        // set the simple frame list
                        for (int i = 0; i < frameList.Length; i++)
                        {
                            ds[MCdicom.SIMPLE_FRAME_LIST, i] = frameList[i];
                        }
                        break;

                    case IMAGE_LEVEL:
                        // We only request one image at a time, but multiple
                        // instance UIDs could be appended at the Image Level
                        ds[MCdicom.SOP_INSTANCE_UID, 0] = result.getValue(MCdicom.SOP_INSTANCE_UID);

                        // We must send the unique keys for levels above
                        if (result.model != COMPOSITE_MODEL)
                        {
                            ds[MCdicom.SERIES_INSTANCE_UID, 0] = result.getValue(MCdicom.SERIES_INSTANCE_UID);
                            ds[MCdicom.STUDY_INSTANCE_UID, 0] = result.getValue(MCdicom.STUDY_INSTANCE_UID);

                            // Study Root Model does not support Patient ID
                            if (result.model != STUDY_ROOT_MODEL)
                                ds[MCdicom.PATIENT_ID, 0] = result.getValue(MCdicom.PATIENT_ID);
                        }
                        break;

                    case SERIES_LEVEL:
                        // We only request one series at a time, but multiple
                        // series UIDs could be appended at the Series Level
                        ds[MCdicom.SERIES_INSTANCE_UID, 0] = result.getValue(MCdicom.SERIES_INSTANCE_UID);

                        // We must send the unique keys for levels above
                        ds[MCdicom.STUDY_INSTANCE_UID, 0] = result.getValue(MCdicom.STUDY_INSTANCE_UID);

                        // Study Root Model does not support Patient ID
                        if (result.model != STUDY_ROOT_MODEL)
                            ds[MCdicom.PATIENT_ID, 0] = result.getValue(MCdicom.PATIENT_ID);
                        break;

                    case STUDY_LEVEL:
                        // We only request one study at a time, but multiple
                        // study UIDs could be appended at the Study Level
                        ds[MCdicom.STUDY_INSTANCE_UID, 0] = result.getValue(MCdicom.STUDY_INSTANCE_UID);

                        // We must send the unique keys for levels above
                        // Study Root Model does not support Patient ID
                        if (result.model != STUDY_ROOT_MODEL)
                            ds[MCdicom.PATIENT_ID, 0] = result.getValue(MCdicom.PATIENT_ID);
                        break;

                    default: // PATIENT_LEVEL 
                        // Note: the standard only allows one patient ID
                        //      per C-MOVE request
                        ds[MCdicom.PATIENT_ID, 0] = result.getValue(MCdicom.PATIENT_ID);
                        break;
                }
            }
            catch (Exception e)
            {
                Util.printError("buildCMOVErequest: Unable to set Unique Key values", e);
                return false;
            }

            return true;
        }
        /// <summary> Send C-MOVE-RQ message to the Query/Retrieve SCP.  We request that the
        /// image object be moved to our Storage Service Class SCP.  Note that main
        /// started the Storage Service SCP thread at startup.
        /// </summary>
        /// <param name="result">Contains query fields defining the object to be moved.
        /// </param>
        /// <param name="assoc">The association on which to send the request.
        /// </param>
        /// <returns>true if all went well.
        /// </returns>
        public bool SendCMOVEmessage(ResultFields result, MCassociation assoc)
        {
            MCdataSet ds = null;
            MCdimseMessage msg = null;

            // We proceed only if Query was sucessful
            if (!OkToMove(result)) return true; 

            // We are using the Query/Retrieve Service Class
            MCqueryRetrieveService service = new MCqueryRetrieveService(assoc);

            String serviceName = "";

            // Construct a data set to use with the C-MOVE request
            try
            {
                if (result.model == COMPOSITE_MODEL) serviceName = "COMPOSITE_INSTANCE_ROOT_RET_MOVE";
                else serviceName = result.modelName + "_QR_MOVE";
                ds = new MCdataSet(MCdimseService.C_MOVE_RQ, serviceName);
            }
            catch (Exception e)
            {
                Util.println("Unable to create data set for C-MOVE-RQ.");
                Util.printError("Is " + serviceName + " configured?", e);
                return false;
            }

            // Build the C-MOVE identifier
            if (!BuildCMOVEidentifier(ds, result)) return false;

            // Validate the message
            ValMessage(ds, "C-MOVE-RQ");

            // Send the C-MOVE-RQ message
            try
            {
                msg = service.sendMoveRequest(ds, moveDestinationTitle);
            }
            catch (Exception e)
            {
                Util.printError("sendCMOVEmessage: sendMoveRequest error", e);
                return false;
            }

            // After sending the C-MOVE-RQ, we wait on response messages (which may be
            // pending) until we have a success/failure response back from the server.
            bool done = false;
            while (!done)
            {
                MCdimseMessage responseMsg = null;

                // Read response message from SCP
                try
                {
                    responseMsg = assoc.read(timeout);

                    if (responseMsg == null)
                    {
                        Util.println("Read timed out - trying again.");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Util.printError("sendCFINDmessage: Error on network read", e);
                    return false;
                }

                // Retrieve and examine the response status
                ushort response;
                try
                {
                    response = responseMsg.ResponseStatus;
                }
                catch (Exception e)
                {
                    Util.printError("sendCMOVEmessage: Error on network read", e);
                    return false;
                }
                // Creates object to return move responses
                MoveResponse moveResponse = new MoveResponse();
                switch (response)
                {
                    case MCdimseService.C_MOVE_SUCCESS_NO_FAILURES:
                        Util.println("Move Response is C_MOVE_SUCCESS_NO_FAILURES");                        
                        Util.println("  Completed sub-operations: " + responseMsg.CompletedSubOperations);
                        Util.println("  Failed sub-operations: " + responseMsg.FailedSubOperations);
                        Util.println("  Warning sub-operations: " + responseMsg.WarningSubOperations);
                        moveResponse.Complete = responseMsg.CompletedSubOperations;
                        moveResponse.Failed = responseMsg.FailedSubOperations;
                        moveResponse.Warnings = responseMsg.WarningSubOperations;
                        done = true;
                        break;

                    case MCdimseService.C_MOVE_PENDING_MORE_SUB_OPERATIONS:
                        Util.println("Pending response to C-MOVE Received:");
                        Util.println("  Remaining sub-operations: " + responseMsg.RemainingSubOperations);
                        Util.println("  Completed sub-operations: " + responseMsg.CompletedSubOperations);
                        Util.println("  Failed sub-operations: " + responseMsg.FailedSubOperations);
                        Util.println("  Warning sub-operations: " + responseMsg.WarningSubOperations);
                        moveResponse.Remaining = responseMsg.RemainingSubOperations;
                        break;

                    default:
                        Util.println("ERROR: Unknown response to C-MOVE request: " + response);
                        Util.println("  Remaining sub-operations: " + responseMsg.RemainingSubOperations);
                        Util.println("  Completed sub-operations: " + responseMsg.CompletedSubOperations);
                        Util.println("  Failed sub-operations: " + responseMsg.FailedSubOperations);
                        Util.println("  Warning sub-operations: " + responseMsg.WarningSubOperations);
                        moveResponse.Complete = responseMsg.CompletedSubOperations;
                        moveResponse.Failed = responseMsg.FailedSubOperations;
                        moveResponse.Warnings = responseMsg.WarningSubOperations;
                        done = true;
                        break;
                }

            } // while not done - loop back to read another request message

            return true;
        }
        /// <summary> Ask user if it's OK to move the image object(s) requested.
        /// </summary>
        /// <param name="result">Contains the query fields of the image(s) to be moved.
        /// </param>
        /// <returns>true if user says "Yes".
        /// </returns>
        public bool OkToMove(ResultFields result)
        {            
            switch (result.level)
            {
                case IMAGE_LEVEL:
                case FRAME_LEVEL:
                    Util.println("Image Number " + result.getValue(MCdicom.IMAGE_NUMBER));
                    break;

                case SERIES_LEVEL:
                    Util.println("Series Number " + result.getValue(MCdicom.SERIES_NUMBER));
                    break;

                case STUDY_LEVEL:
                    Util.println("Study ID " + result.getValue(MCdicom.STUDY_ID));
                    break;

                default:  // PATIENT
                    Util.println("Study ID " + result.getValue(MCdicom.PATIENT_ID));
                    break;
            }
            if (examPresent) return true; 
            return false;
        } 
        /// <summary> Validates a message and print any validation errors found.
        /// </summary>
        /// <param name="ds">The data set to validate.
        /// </param>
        /// <param name="msgDesc">Describes the message being validated.
        /// </param>
        public void ValMessage(MCdataSet ds, String msgDesc)
        {
            if (ds.validate(MCvalidationLevel.Errors_And_Warnings))
            {
                Util.println(msgDesc + " message validated!");
                return;
            }

            Util.println("\n***\tVALIDATION ERROR: " + msgDesc);
            Util.println("(grp ,elem) Value# Type  Message");

            MCvalidationError ve = ds.getNextValidationError();
            while (ve != null)
            {
                String type = null;
                if (ve.ErrorNumber == MCexception.UNABLE_TO_CHECK_CONDITION) type = "I";
                else if (ve.ErrorNumber == MCexception.NOT_ONE_OF_DEFINED_TERMS
                    || ve.ErrorNumber == MCexception.NON_SERVICE_ATTRIBUTE) type = "W";
                else type = "E";

                Util.print(new MCtag(ve.Tag) + "   " + type);
                Util.printInColumn(ve.ValueNumber, 2);
                Util.print("   ");
                Util.println(ve.ErrorDescription);

                ve = ds.getNextValidationError();
            }
        }
    }
}
