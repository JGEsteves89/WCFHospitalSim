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
    /// This class is responsible for handle all common operations on
    /// communication with the provider
    /// </summary>
    public class MergeBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MergeBase()
        {
        
            Timeout = 3000000;
        }

        //  Constant values - Definition of the argument values for the exit() function
        private const int EXIT_SUCCESS = 0;
        private const int EXIT_FAILURE = 1;

        /// <summary>
        /// Association with the remote application
        /// </summary>
        protected static MCassociation myAssoc = null;

        #region Atributes

        /// <summary>
        /// Communications Timeout
        /// </summary>
        [DefaultValue(300000)]
        public long Timeout
        {
            get; set;
        }

        /// <summary>
        /// Configuration Object
        /// </summary>
        public MergeHandler Handler
        {
            get;
            set;
        }

        #endregion

        #region Association

        /// <summary>
        /// Opens the association with provider. It is necessary 
        /// to send the information
        /// </summary>
        protected void OpenAssociation()
        {
            try
            {
                MCremoteApplication remApp;
                try
                {
                    if (Handler.RemoteHost != null)
                        remApp = new MCremoteApplication(Handler.RemoteAE, new IPEndPoint(Dns.GetHostEntry(Handler.RemoteHost).AddressList[0], Handler.RemotePort), Handler.SCUContextList);
                    else
                        remApp = MCremoteApplication.getObject(Handler.RemoteAE);

                    //if (Handler.SecureAssociation)
                    //{
                    //    Ssl ssl = new Ssl();

                    //    ssl.Certificate = "ssl.crt";
                    //    ssl.Password = "SSL SAMPLE";

                    //    myAssoc = MCassociation.requestSecureAssociation(Handler.LocalApplication, remApp, ssl);
                    //}
                    //else
                    //{
                        myAssoc = MCassociation.requestAssociation(Handler.LocalApplication, remApp);
                    //}
                }
                catch (MCexception e1)
                {
                    Util.println("Cannot create an association with the remote application");
                    throw e1;
                }
            }
            catch (MCexception)
            {
                throw new System.IO.IOException();
            }
        }

        /// <summary>
        /// Close the association
        /// </summary>
        protected void CloseAssociation()
        {
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

        #endregion
    }
}
