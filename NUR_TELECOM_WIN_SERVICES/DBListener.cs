using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    class DBListener
    {
        public void CheckList()
        {
            List<AccessInformation> acceses = DBConnector.get_scud_information();

            acceses.ForEach(x => x.setNtEvetnType());

            acceses = acceses.Where(x => x.SHORT_COMMENT != null && x.SHORT_COMMENT.Contains('/') && x.ntEventType!=null).ToList();



            for (int i = 0; i < acceses.Count; i++)
            {
                //acceses[i].setNtEvetnType();
                acceses[i].sendInformation();
                // DBConnector.get_scud_information();
                //DBConnector.test(acceses[i]);
            }


        }

        public void start()
        {
            while (true)
            {
                CheckList();
                Thread.Sleep(5000);
            }

        }

    }
}
