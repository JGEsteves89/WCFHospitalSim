using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace ADIU
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            /// 
            MergeHandler handler = MergeHandler.Instance;
            handler.RemoteHost = null;
            handler.LicenseNum = System.Configuration.ConfigurationManager.AppSettings["LicenseNum"];
            handler.RemotePort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RemotePort"]);
            handler.RemoteAE = System.Configuration.ConfigurationManager.AppSettings["RemoteAE"];
            handler.LocalAE = System.Configuration.ConfigurationManager.AppSettings["LocalAE"];
            handler.IniFilePath = System.Configuration.ConfigurationManager.AppSettings["IniFilePath"];
            handler.SecureAssociation = false;
            handler.Initialize();
            handler.RegisterApp();
            handler.CreateContextList();

            WorkList workList = new WorkList();
            workList.Handler = MergeHandler.Instance;
            List<WorkPatient> workPatients = workList.GetList();

            for (int i = 0; i < workPatients.Count; i++)
            {
                Console.WriteLine(i + " - \t" + workPatients[i].ToString());

            }
            Console.Write("Choose one patient: ");
            string nbr = Console.ReadLine();
            int nbrPatient = 0;

            if (int.TryParse(nbr, out nbrPatient))
            {
                WorklistProgress progress = new WorklistProgress();
                progress.Handler = MergeHandler.Instance;
                progress.Patient =  workPatients[nbrPatient];
                progress.CreateProgress();

                progress.SetProgress("IN PROGRESS");

                progress.SendNSETRQComplete();
            }
            //workPatients.ForEach(r => Console.WriteLine(r.ToString()));


            Console.ReadLine();

            /*
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(ServicesToRun);*/
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
            new Service()
            };

            if (Environment.UserInteractive)
            {
                RunInteractive(servicesToRun);
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }
        }

        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            String nameService = string.Empty;
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
            BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("{0} Started", service.ServiceName);
                nameService = service.ServiceName;
            }
            Console.WriteLine("Press any key to stop the service {0}", nameService);

            Console.Read();
            Console.WriteLine();
            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
            BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("{0} Stopped", service.ServiceName);
            }
            Thread.Sleep(1000);

        }
    }
}
