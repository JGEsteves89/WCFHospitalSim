using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.ServiceProcess;

namespace ADIU
{
    /// <summary>
    /// 
    /// </summary>
    [RunInstaller(true)]
    public partial class InstallerADIU : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        /// <summary>
        /// 
        /// </summary>
        public InstallerADIU()
        {
            InitializeComponent();

            process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            service = new ServiceInstaller
            {
                ServiceName = "ADI WCF Service"
            };

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
