using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    class StatIdClass
    {
        public static int lastId { get; set; }
        public StatIdClass()
        {
            try
            {
                using (StreamReader sr = new StreamReader("lastId.txt"))
                {
                    lastId = Convert.ToInt32(sr.ReadLine());
                }

            }
            catch (Exception ex)
            {
                lastId = 0;
            }
            
        }
        public static void fixChnges()
        {
            using(StreamWriter sw=new StreamWriter("lastId.txt"))
            {
                sw.WriteLine(lastId);
            }
        }

    }
}
