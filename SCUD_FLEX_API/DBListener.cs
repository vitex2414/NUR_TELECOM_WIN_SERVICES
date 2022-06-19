using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    class DBListener
    {

        public void execCommands()
        {
            List<Commands> lis = DBConnector.getUnexecutedCOmmands();

            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].isCorrect())
                {
                    string result = DBConnector.executeCommand(lis[i].command);
                    DBConnector.fixResponse(result, lis[i].id, 1);
                }
                else
                {
                    DBConnector.fixResponse("none", lis[i].id, 2);
                }
            }

        }


        public void start()
        {
            while (true)
            {
                int sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["sleepingTime"]);
                execCommands();
                Thread.Sleep(sleepTime);
            }
        }
    }
}
