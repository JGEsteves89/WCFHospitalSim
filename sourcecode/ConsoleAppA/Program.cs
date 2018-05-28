using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppA
{
    class Program
    {
        static void Main(string[] args)
        {

            //Step 1: Create an instance of the WCF proxy.  
            ServiceReferenceB.ServiceBClient client = new ServiceReferenceB.ServiceBClient();

            // Step 2: Call the service operations.  
            // Call the Add service operation.  
            string result = client.GetData(1);
            Console.WriteLine("I am console A and I send '1' para consola B");
            Console.WriteLine("Recieved '{0}'", result);
            client.Close();

            Console.ReadKey();
        }
    }
}
