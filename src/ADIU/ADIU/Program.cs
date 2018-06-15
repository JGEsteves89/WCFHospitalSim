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
        /// Start the ADI Services
        /// </summary>
        static void Main(String[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ADIService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}