using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    class WebMethod
    {
        private string uri;
        private string body;
        string token;

        public WebMethod() { }
        public WebMethod(string uri, string body)
        {
            this.uri = uri;
            this.body = body;
        }

        public string GetAuthToken()
        {

            string authBdy = @"{
    ""username"":""{0}"", 
    ""password"":""{1}""
}";
            string uri = ConfigurationManager.AppSettings["authAdr"];

            authBdy = authBdy.Replace("{0}", ConfigurationManager.AppSettings["nLogin"]).Replace("{1}", ConfigurationManager.AppSettings["nPass"]);

            WebMethod m = new WebMethod(uri, authBdy);
           

            string authReponse = m.POSTmethod(0, 1);//POSTmethod(1, 1);
            JObject response = JObject.Parse(authReponse);
            token = response["result"].ToString();
            return token;
        }



        public string POSTmethod(int token, int proxyType)
        {
            string res = "";

            var data = Encoding.GetEncoding("UTF-8").GetBytes(body);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "POST";

            //Если тип прокси 0 то запрос отправляется на ВН сервис иначе на внешний 
            if (proxyType == 0)
            {
                req.Proxy = new WebProxy() { UseDefaultCredentials = true };
            }
            else
            {
                WebProxy myProxy = new WebProxy();
                req.Proxy = new WebProxy(ConfigurationManager.AppSettings["proxyServer"]);
                req.Proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["proxyLogin"], ConfigurationManager.AppSettings["proxyPassword"]);
            }
            //Если токен пуст не передается в заголовки
            if (token == 1)
            {
                req.Headers.Add("Authorization", String.Format("Bearer {0}", token));
            }
            req.ContentType = "application/json";
            req.ContentLength = data.Length;

            // Ответ сервиса
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            try
            {
                //Запись ответа от сервиса
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)req.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
                res = responseString.ToString();
            }
            catch (Exception ex)
            {
                res = "Ошибка подключения к сервису: " + ex.Message;
            }
            return res;
        }

        public string PUTmethod(int token, int proxyType)
        {
            string res = "";

            var data = Encoding.GetEncoding("UTF-8").GetBytes(body);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "PUT";

            //Если тип прокси 0 то запрос отправляется на ВН сервис иначе на внешний 
            if (proxyType == 0)
            {
                req.Proxy = new WebProxy() { UseDefaultCredentials = true };
            }
            else
            {
                WebProxy myProxy = new WebProxy();
                req.Proxy = new WebProxy(ConfigurationManager.AppSettings["proxyServer"]);
                req.Proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["proxyLogin"], ConfigurationManager.AppSettings["proxyPassword"]);
            }
            //Если токен пуст не передается в заголовки
            if (token == 1)
            {
                req.Headers.Add("Authorization", String.Format("Bearer {0}", this.token));
            }
            req.ContentType = "application/json";
            req.ContentLength = data.Length;

            // Ответ сервиса
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            try
            {
                //Запись ответа от сервиса
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)req.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
                res = responseString.ToString();
            }
            catch (Exception ex)
            {
                res = "Ошибка подключения к сервису: " + ex.Message;
            }
            return res;
        }


    }
}
