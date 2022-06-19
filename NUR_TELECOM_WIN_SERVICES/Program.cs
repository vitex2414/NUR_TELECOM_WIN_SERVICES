using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {

            StatIdClass lastId=new StatIdClass();
#if DEBUG
            Service1 ser = new Service1();
            ser.OnDeb();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
            
#endif
        }
    }
}
