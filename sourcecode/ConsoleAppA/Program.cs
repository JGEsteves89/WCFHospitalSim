using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibray;

namespace ConsoleAppA
{
    /// <summary>
    /// Test the connection to the Service A
    /// </summary>
    class Program
    {
        //Create a client to the service A
        static ServiceReferenceA.ServiceAClient srvA = new ServiceReferenceA.ServiceAClient();

        static void Main(string[] args)
        {
            bool exit = false;
            string command = string.Empty;
            Console.WriteLine("Started App A!\n");
        
            do
            {
                //Read Command 
                Console.Write("Insert command:");
                command = Console.ReadLine();

                //Executes the function relative to command
                switch(command)
                {
                    case "test":
                        {
                            TestConnection();
                            Console.WriteLine();
                            break;
                        }

                    case "list":
                        {
                            List<Patient> patients = GetPatientsFromB();

                            foreach (Patient pat in patients)
                            {
                                Console.WriteLine("Pat: {0}, Age: {1}", pat.Name, pat.Age);
                            }
                            Console.WriteLine();
                        }
                        break;

                    case "exit":
                            exit = true;
                        break;
                    default:
                        {
                            Console.WriteLine("Command invalid");
                            Console.WriteLine();
                            break;
                        }
                }
            } while (!exit);
        }

        /// <summary>
        /// Gets the patients in Service B through service A
        /// </summary>
        /// <returns>Returns a list of patients</returns>
        private static List<Patient> GetPatientsFromB()
        {
            if (srvA.ConnectionOK())
            {
                return srvA.getPatientsFromB().ToList();
            }
            Console.WriteLine("This application cannot connect with service A");
            return new List<Patient>();
        }

        /// <summary>
        /// Function verifies if conection with service B in service A is ok.
        /// Show the state in console
        /// </summary>
        private static void TestConnection()
        {
            if (srvA.ConnectionOK())
                Console.WriteLine("Connection to Service A Successful ");
            else
                Console.WriteLine("Connection to Service A not successful ");
        }
    }
}
