﻿using System;
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
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(ServicesToRun);


            //ServiceBase[] servicesToRun = new ServiceBase[]
            //{
            //new Service()
            //};

            //if (Environment.UserInteractive)
            //{
            //    RunInteractive(servicesToRun);
            //}
            //else
            //{
            //    ServiceBase.Run(servicesToRun);
            //}
        }

        //private static void RunInteractive(ServiceBase[] servicesToRun)
        //{
        //    String nameService = string.Empty;
        //    MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
        //    BindingFlags.Instance | BindingFlags.NonPublic);
        //    foreach (ServiceBase service in servicesToRun)
        //    {
        //        Console.Write("Starting {0}...", service.ServiceName);
        //        onStartMethod.Invoke(service, new object[] { new string[] { } });
        //        Console.Write("{0} Started", service.ServiceName);
        //        nameService = service.ServiceName;
        //    }
        //    Console.WriteLine("Press any key to stop the service {0}", nameService);

        //    Console.Read();
        //    Console.WriteLine();
        //    MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
        //    BindingFlags.Instance | BindingFlags.NonPublic);
        //    foreach (ServiceBase service in servicesToRun)
        //    {
        //        Console.Write("Stopping {0}...", service.ServiceName);
        //        onStopMethod.Invoke(service, null);
        //        Console.WriteLine("{0} Stopped", service.ServiceName);
        //    }
        //    Thread.Sleep(1000);
        //}
    }
}
