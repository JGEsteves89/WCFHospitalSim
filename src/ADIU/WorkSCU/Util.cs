using System;
using System.Collections;

using Mergecom;
using Mergecom.Exceptions;

namespace WorkSCU
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
        /// Reads a line from stdin, insures that it is at least
        /// one character long, and returns it trimmed.
        /// </summary>
        /// <value>Trimmed input
        /// </value>
        public static String Line
        {
            get
            {
                String userInput = String.Empty;

                try
                {
                    while (userInput.Length == 0)
                    {
                        userInput = Console.ReadLine();
                    }
                }
                catch
                {
                    Console.WriteLine("IO Error reading from console");
                    userInput = String.Empty;
                }
                return (userInput == null) ? String.Empty : userInput.Trim();
            }
        }

        /// <summary>
        /// Retrieves the first character typed by the user.
        /// Keeps trying until user types one of the expected characters.
        /// </summary>
        /// <param name="expected">An array containing expected characters.
        /// </param>
        /// <returns>The character typed by the user.
        /// </returns>
        public static char getChar(ArrayList expected)
        {
            for (; ; )
            {
                String inp = Line;
                if (inp.Length == 0) continue;

                for (int i = 0; i < expected.Count; i++)
                {
                    if (inp[0] == (char)expected[i]) return inp[0];
                }
            }
        }

        /// <summary>
        /// Retrieves the first character typed by the user.
        /// Keeps trying until user types one of the expected characters.
        /// </summary>
        /// <param name="expected">An array containing expected characters.
        /// </param>
        /// <returns>The character typed by the user.
        /// </returns>
        public static char getChar(char[] expected)
        {
            return getChar(new ArrayList(expected));
        }

        /// <summary>
        /// This method gets the merge.ini file location from the command line
        /// and uses it to initialize the library.
        /// </summary>
        /// <param name="args">The command line arguments, can be null.
        /// </param>
        internal static void initializeMergeCOM(String[] args)
        {
            for (int i = 0; args != null && i < (args.Length - 1); i++)
            {
                if (args[i].Equals("-i", StringComparison.InvariantCultureIgnoreCase))
                {
                    iniFilePath = args[i + 1];                   
                }
            }

            MC.mcInitialization(new System.IO.FileInfo("C:\\Users\\dcosta\\Documents\\GitHub\\Dummy\\WCFHospitalSim\\src\\ADIU\\ADIU\\bin\\MERGE.INI"), "F47D-4E28-F854");
        }

        /// <summary>
        /// Prints a message to the console.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        internal static void print(String msg)
        {
            Console.Write(msg);
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

        /// <summary>
        /// Prints a number, right-justified in a specified column width.
        /// </summary>
        /// <param name="num">The integer number to print.
        /// </param>
        /// <param name="colWidth">The width of the column being printed.
        /// </param>
        internal static void printInColumn(int num, int colWidth)
        {
            printInColumn(Convert.ToString(num), colWidth);
        }

        /// <summary>
        /// Prints a string and appends spaces to make string the
        /// specified column width.
        /// </summary>
        /// <param name="str">The string to print.
        /// </param>
        /// <param name="colWidth">The width of the column being printed.
        /// </param>
        internal static void printInColumn(String str, int colWidth)
        {
            print(str);
            for (int p = str.Length; p < colWidth; p++) print(" ");
        }

    }
}
