using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DBListener dbl = new DBListener();
            Thread nThread = new Thread(new ThreadStart(dbl.start));
            nThread.Start();
        }

        protected override void OnStop()
        {
            StatIdClass.fixChnges();
        }
        public void OnDeb()
        {
            OnStart(null);
        }

    }
}
