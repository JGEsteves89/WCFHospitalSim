using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientDLL;


namespace ConsoleAppB
{
    class Program
    {
        static ServiceReferenceB.ServiceBClient srvB = new ServiceReferenceB.ServiceBClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Started App B!");
            List<Patient> patients = getPatientsFromA();

            foreach (Patient pat in patients)
            {
                Console.WriteLine("Pat: {0}, Age: {1}",pat.name,pat.age);
            }


            Console.ReadKey();
        }

        private static List<Patient> getPatientsFromA()
        {
            if (srvB.ConnectionOK())
            {
                return srvB.getPatientsFromA().ToList();
            }
            Console.WriteLine("This application cannot connect with service A");
            return new List<Patient>();
        }
    }
}
