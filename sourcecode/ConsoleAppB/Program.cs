using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibray;


namespace ConsoleAppB
{
    class Program
    {
        static ServiceReferenceB.ServiceBClient srvB = new ServiceReferenceB.ServiceBClient();
        static void Main(string[] args)
        {
            bool exit = false;
            string command = string.Empty;
            Console.WriteLine("Started App B!\n");

            do
            {
                Console.Write("Insert command:");
                command = Console.ReadLine();

                switch (command)
                {
                    case "list":
                        {
                            List<Patient> patients = getPatientsFromA();

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

        private static List<Patient> getPatientsFromA()
        {
            if (srvB.ConnectionOK())
            {
                return srvB.getPatientsFromA().ToList();
            }
            Console.WriteLine("This application cannot connect with service B");
            return new List<Patient>();
        }
    }
}
