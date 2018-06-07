using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibray;


namespace ConsoleAppB
{
    /// <summary>
    /// Test the connection to the Service B
    /// </summary>
    class Program
    {
        //Create a client to the service B
        static ServiceReferenceB.ServiceBClient srvB = new ServiceReferenceB.ServiceBClient();
        static void Main(string[] args)
        {
            bool exit = false;
            string command = string.Empty;
            Console.WriteLine("Started App B!\n");

            do
            {
                //Read Command 
                Console.Write("Insert command:");
                command = Console.ReadLine();


                //Executes the function relative to command
                switch (command)
                {
                    case "test":
                        {
                            TestConnection();
                            Console.WriteLine();
                            break;
                        }

                    case "list":
                        {
                            List<Patient> patients = GetPatientsFromA();

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
        /// Gets the patients in Service A through service B
        /// </summary>
        /// <returns>Returns a list of patients</returns>
        private static List<Patient> GetPatientsFromA()
        {
            if (srvB.ConnectionOK())
            {
                return srvB.getPatientsFromA().ToList();
            }
            Console.WriteLine("This application cannot connect with service B");
            return new List<Patient>();
        }

        /// <summary>
        /// Function verifies if conection with service B in service A is ok.
        /// Show the state in console
        /// </summary>
        private static void TestConnection()
        {
            if (srvB.ConnectionOK())
                Console.WriteLine("Connection to Service B Successful ");
            else
                Console.WriteLine("Connection to Service B not successful ");

        }
    }
}
