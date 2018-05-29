using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibray;

namespace ConsoleAppA
{
    class Program
    {
        static ServiceReferenceA.ServiceAClient srvA = new ServiceReferenceA.ServiceAClient();

        static void Main(string[] args)
        {
            bool exit = false;
            string command = string.Empty;
            Console.WriteLine("Started App A!\n");
        
            do
            {
                Console.Write("Insert command:");
                command = Console.ReadLine();

                switch(command)
                {
                    case "list":
                        {
                            List<Patient> patients = getPatientsFromB();
                            foreach (Patient pat in patients)
                            {
                                Console.WriteLine("Pat: {0}, Age: {1}", pat.name, pat.age);
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

        private static List<Patient> getPatientsFromB()
        {
            if (srvA.ConnectionOK())
            {
                return srvA.getPatientsFromB().ToList();
            }
            Console.WriteLine("This application cannot connect with service A");
            return new List<Patient>();
        }
    }
}
