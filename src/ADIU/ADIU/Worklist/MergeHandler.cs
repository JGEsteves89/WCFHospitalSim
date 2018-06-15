using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Mergecom;
using Mergecom.Exceptions;
using System.ComponentModel;


namespace ADIU
{
   /// <summary>
   /// This class is responsible for all configuration
   /// used in the DICOM Merge Configuration
   /// </summary>
    public sealed class MergeHandler
    {
        //  Constant values - Definition of the argument values for the exit() function
        private const int EXIT_SUCCESS = 0;
        private const int EXIT_FAILURE = 1;
        private static readonly MergeHandler _instance = new MergeHandler();

        /// <summary>
        /// Singleton Instance
        /// </summary>
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
            GetConfiguration();
        }

        /// <summary>
        /// License Number
        /// </summary>
        public string LicenseNum
        {
            get;
            set;
        }

        /// <summary>
        /// Remote Application Entity
        /// </summary>
        public string RemoteAE
        {
            get;
            set;
        }

        /// <summary>
        /// Local Application Entity
        /// </summary>
        public string LocalAE
        {
            get;
            set;
        }

        /// <summary>
        /// Merge.Ini File Path
        /// </summary>
        public string IniFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Remote Host IP
        /// </summary>
        public string RemoteHost
        {
            get;
            set;
        }

        /// <summary>
        /// Remote Port from the Host
        /// </summary>
        public int RemotePort
        {
            get;
            set;
        }

        /// <summary>
        ///  Set by main and never changed
        /// </summary>
        public MCapplication LocalApplication
        {
            get;
            private set;
        }

        /// <summary>
        /// Context List configuration-
        /// Little Endian, Big Endian
        /// </summary>
        public MCproposedContextList SCUContextList
        {
            get;
            private set;
        }

        /// <summary>
        /// SSL Comunnication
        /// </summary>
        public bool SecureAssociation
        {
            get;
            set;
        }

        /// <summary>
        /// Get All configuration from the App.Config
        /// </summary>
        public void GetConfiguration()
        {
            try
            {
                RemoteHost = null;
                LicenseNum = System.Configuration.ConfigurationManager.AppSettings["LicenseNum"];
                RemotePort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RemotePort"]);
                RemoteAE = System.Configuration.ConfigurationManager.AppSettings["RemoteAE"];
                LocalAE = System.Configuration.ConfigurationManager.AppSettings["LocalAE"];
                IniFilePath = System.Configuration.ConfigurationManager.AppSettings["IniFilePath"];
                SecureAssociation =
                    Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["SecureAssociation"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Initialize the Merge DICOM library
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

        /// <summary>
        /// Get the application from the Merge Application
        /// </summary>
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

        /// <summary>
        /// Create the Context List
        /// </summary>
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
