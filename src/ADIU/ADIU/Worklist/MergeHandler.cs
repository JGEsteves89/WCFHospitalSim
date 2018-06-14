using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;
using System.ComponentModel;


namespace ADIU
{
   
    public sealed class MergeHandler
    {
        //  Constant values - Definition of the argument values for the exit() function
        private const int EXIT_SUCCESS = 0;
        private const int EXIT_FAILURE = 1;
        private static readonly MergeHandler _instance = new MergeHandler();

        public static MergeHandler Instance
        {
            get
            {
                return _instance;
            }
        }

        private MergeHandler()
        {
            // place for instance initialization code

            RemoteHost = null;
            RemotePort = 104;
        }

        public string LicenseNum
        {
            get;
            set;
        }

        public string RemoteAE
        {
            get;
            set;
        }

        public string LocalAE
        {
            get;
            set;
        }

        public string IniFilePath
        {
            get;
            set;
        }

        public string RemoteHost
        {
            get;
            set;
        }

        public int RemotePort
        {
            get;
            set;
        }

        // Set by main and never changed
        public MCapplication LocalApplication
        {
            get;
            private set;
        }

        public MCproposedContextList SCUContextList
        {
            get;
            private set;
        }


        public bool SecureAssociation
        {
            get;
            set;
        }

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

        public void RegisterApp()
        {
            // ------------------------------------------------------------------------
            // Register this DICOM application
            // ------------------------------------------------------------------------
            try
            {
                LocalApplication = MCapplication.getApplication(LocalAE);
            }
            catch (System.Exception e)
            {
                Util.printError("Unable to register \"" + LocalAE + "\".", e);
                System.Environment.Exit(EXIT_FAILURE);
            }
        }

        public void CreateContextList()
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

                SCUContextList = new MCproposedContextList("SampleWorkScuServices", contextArray);
            }
            catch (MCexception e)
            {
                Util.printError("Unable to create Worklist context", e);
            }
        }

    }
}
