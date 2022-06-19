using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {

        ServiceInstaller serviceinstaller;
        ServiceProcessInstaller proccessInstaller;
        public Installer1()
        {
            InitializeComponent();

            serviceinstaller = new ServiceInstaller();
            proccessInstaller = new ServiceProcessInstaller();
            proccessInstaller.Account = ServiceAccount.LocalSystem;
            serviceinstaller.StartType = ServiceStartMode.Automatic;
            serviceinstaller.ServiceName = "ScudAPI";
            serviceinstaller.Description = "Служба предоставления информации из СКУД";
            Installers.Add(proccessInstaller);
            Installers.Add(serviceinstaller);

        }
    }
}
