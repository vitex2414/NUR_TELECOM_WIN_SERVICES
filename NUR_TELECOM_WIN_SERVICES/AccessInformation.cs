using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    class AccessInformation
    {
        public string DEVICE_SID { get; set; }
        public string EVENT_TYPE_ID { get; set; }
        public string EVENT_TIME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public int ID { get; set; }
        public string SHORT_COMMENT { get; set; }



        // public string ourEventType { get; set; }
        public string ntEventType { get; set; }

        public void setNtEvetnType()
        {
            if (ConfigurationManager.AppSettings["in"] == EVENT_TYPE_ID)
            {
                ntEventType = "IN";
            }
            else if (ConfigurationManager.AppSettings["out"] == EVENT_TYPE_ID)
            {
                ntEventType = "OUT";
            }
        }


        public void sendInformation()
        {
            string bdy = @"[{
    ""user"": ""{user}"", 
    ""in_out_event"": [{
        ""controller_id"": ""{controller}"",
        ""date_time"": ""{dateTime}"",
        ""type"": ""{type}""
    }]
}]";


            //2021-03-19 08:45:12
            bdy = bdy.Replace("{user}", SHORT_COMMENT).Replace("{controller}", DEVICE_SID).Replace("{dateTime}", Convert.ToDateTime(EVENT_TIME).ToString("yyyy-MM-dd HH:mm:ss")).Replace("{type}", ntEventType);

            WebMethod mthd = new WebMethod(ConfigurationManager.AppSettings["scudAdr"], bdy);
            mthd.GetAuthToken();

            string resp = mthd.PUTmethod(1, 1);
            string status = "none";
            try
            {
                JObject respObj = JObject.Parse(resp);

                status = respObj["resultCode"].ToString();

            }
            catch(Exception ex)
            {
                status = "none";
            }

            DBConnector.NRT_FIX_COLLECTORS_REQ(bdy, resp, status);




        }

    }
}
