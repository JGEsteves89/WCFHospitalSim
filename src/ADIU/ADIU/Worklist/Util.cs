using System;
using System.Collections;

using Mergecom;
using Mergecom.Exceptions;

namespace ADIU
{
    /*
    * Copyright © 2006 Merge OEM Corporation, a Merge Healthcare Company.
    * All rights reserved Cedara proprietary/confidential. Use is subject to 
    * license terms.
    */

    /// <summary>Sample programs - Utility Class.  Provides utility methods that
    /// may be used by more than one sample application.
    /// </summary>
    class Util
    {
        /// <summary>
        /// Counter used when creating unique IDs (see createInstanceUid)
        /// </summary>
        private static volatile short uidCounter = 1;

        /// <summary>
        /// The location of the Mergecom ini file.
        /// </summary>
        public static String iniFilePath = "merge.ini";

        /// <summary>
        /// Creates a SOP instance UID.
        /// </summary>
        /// <remarks>
        /// This method creates a new UID for use within these sample 
        /// applications. Note that this is not a valid method
        /// for creating UIDs within DICOM because the "base UID"
        /// is not valid.  
        /// UID Format: baseuid.deviceidentifier.serial_number.current_date.current_time.counter
        /// </remarks>
        public static String createInstanceUid()
        {
            return createInstanceUid(null);
        }

        /// <summary>
        /// Creates a SOP instance UID.
        /// </summary>
        /// <remarks>
        /// This method creates a new UID for use within these sample 
        /// applications. Note that this is not a valid method
        /// for creating UIDs within DICOM because the "base UID"
        /// is not valid.  
        /// UID Format: baseuid.deviceidentifier.serial_number.current_date.current_time.counter
        /// </remarks>
        /// <param name="baseuid">The first part of the UID.
        /// </param>
        public static String createInstanceUid(String baseuid)
        {
            String deviceType = "1";
            String serial = "1";

            if (baseuid == null) baseuid = "2.16.840.1.999999";

            DateTime dt = DateTime.Now;
            String timePortion = String.Format("{0:D4}{1:D2}{2:D2}.{3:D2}{4:D2}{5:D2}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

            return String.Format("{0}.{1}.{2}.{3}.{4}", baseuid, deviceType, serial, timePortion, uidCounter++);
        }

        /// <summary>
        /// Prints an error to the console.
        /// </summary>
        /// <param name="msg">Error message to display.</param>
        /// <param name="e">The exception to display.</param>
        internal static void printError(String msg, Exception e)
        {
            // Prefix message with thread name
            if (msg != null) printMsg(msg);
            if (e != null) println(e.ToString());
        }

        /// <summary>
        /// Displays a message on the console.
        /// </summary>
        /// <param name="msg">The message to display (carriage return appended).</param>
        internal static void println(String msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Displays the current thread and a message to the console.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        internal static void printMsg(String msg)
        {
            //  Prefix message with thread name
            println(System.Threading.Thread.CurrentThread.Name + ": " + msg);
        }


    }
}
