using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using System.Configuration.Install;

namespace ADIU
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Service : ServiceBase
    {
        /// <summary></summary>
        public ServiceHost serviceHost = null;

        /// <summary>
        /// 
        /// </summary>
        public Service()
        {
            InitializeComponent();
            // Name the Windows Service
            ServiceName = "ADI Service - Island Acuity";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                if (serviceHost != null)
                {
                    serviceHost.Close();
                }

                // Create a ServiceHost for the CalculatorService type and 
                // provide the base address.
                serviceHost = new ServiceHost(typeof(ADIUService));

                // Open the ServiceHostBase to create listeners and start 
                // listening for messages.
                serviceHost.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                if (serviceHost != null)
                {
                    serviceHost.Close();
                    serviceHost = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
