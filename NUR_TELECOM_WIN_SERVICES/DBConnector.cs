using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUR_TELECOM_WIN_SERVICES
{
    class DBConnector
    {
        //static int lastId = 0;

        public static List<AccessInformation> get_scud_information()
        {
            OleDbConnection conect = new OleDbConnection(ConfigurationManager.AppSettings["dbFile"]);
            string dtn = DateTime.Now.ToString("dd.MM.yyyy");
            //dtn = "03.02.2020";
            string kv = ConfigurationManager.AppSettings["keyV"];
            string cmd = "";//@"SELECT EVENTS.*, USERS.* FROM (USERS INNER JOIN EVENTS ON USERS.SID = EVENTS.USER_SID) WHERE (dateValue(EVENTS.EVENT_TIME) = dateValue('" + dtn + "')) AND (USERS.KEY_NUMBER = " + kv + ") and EVENTS.ID>" + lastId + "";


            cmd = @"SELECT     EVENTS.DEVICE_SID, EVENTS.EVENT_TYPE_ID, EVENTS.EVENT_SUBTYPE_ID, EVENTS.EVENT_TIME, EVENTS.ADD_INFO, USERS.SID, USERS.FIRST_NAME, USERS.MIDDLE_NAME, USERS.LAST_NAME, USERS.USER_POSITION, GROUPS.NAME, EVENTS.ID, USERS.SHORT_COMMENT
FROM         ((USERS INNER JOIN
                      EVENTS ON USERS.SID = EVENTS.USER_SID) INNER JOIN
                      GROUPS ON USERS.GROUP_SID = GROUPS.SID)
WHERE     (dateValue(EVENTS.EVENT_TIME) = dateValue('"+dtn+"')) AND (GROUPS.NAME = '"+ConfigurationManager.AppSettings["group"] + "') and EVENTS.ID>" + StatIdClass.lastId + "";

           // string cmd = @"SELECT * From USERS";

            string response = executeCommand(cmd);

            List<AccessInformation> lis = JsonConvert.DeserializeObject<List<AccessInformation>>(response);

            if(lis.Count>0)
            StatIdClass.lastId= lis.Max(x => x.ID);

            return lis;

            #region
            //List<AccessInformation> lis = new List<AccessInformation>();

            //OleDbCommand com = new OleDbCommand(cmd, conect);

            //try
            //{
            //    conect.Open();
            //    OleDbDataReader dr = com.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        int id = Convert.ToInt32(dr["ID"]);
            //        if (id > lastId)
            //        {
            //            lastId = id;
            //        }
            //        AccessInformation accInf = new AccessInformation();
            //        accInf.eventTime = dr["EVENT_TIME"].ToString();
            //        accInf.firstName = dr["FIRST_NAME"].ToString();
            //        accInf.lastName = dr["LAST_NAME"].ToString();
            //        accInf.keyNumber = dr["KEY_NUMBER"].ToString();
            //        accInf.ourEventType = dr["EVENT_TYPE_ID"].ToString();
            //        accInf.setNtEvetnType();
            //        //accInf.ntEventType
            //        lis.Add(accInf);


            //    }
            //}
            //catch (Exception ex)
            //{
            //    using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["dir"] + "\\Dbexcpt.txt"))
            //    {
            //        sw.Write(ex.Message);
            //    }
            //}
            //finally
            //{
            //    conect.Close();
            //}
            //return lis;
            #endregion
        }

        public static void NRT_FIX_COLLECTORS_REQ(string request,string response, string status)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            OracleCommand com = new OracleCommand("NRT_FIX_COLLECTORS_REQ", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add("ins_request", OracleDbType.Varchar2, request, System.Data.ParameterDirection.Input);
            com.Parameters.Add("ins_resp", OracleDbType.Clob, response, System.Data.ParameterDirection.Input);
            com.Parameters.Add("ins_status", OracleDbType.Varchar2, status, System.Data.ParameterDirection.Input);


            try
            {
                con.Open();
                com.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }

        }

        public static void test(AccessInformation acInf)
        {
            OracleConnection con = new OracleConnection("DATA SOURCE=192.168.12.232:1521/HBKportal;PASSWORD=NURTELECOM;PERSIST SECURITY INFO=True;USER ID=NURTELECOM");
            string insrStr = "";
            //string insrStr = acInf.eventTime + "#" + acInf.keyNumber + "#" + acInf.lastName + "#" + acInf.ourEventType + "#" + acInf.ntEventType;
            OracleCommand com = new OracleCommand("insert into TEST_ACCESS(INFORMATION) values('" + insrStr + "')", con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }

        }

        static string executeCommand(string cmd)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            OracleCommand com = new OracleCommand("FLEX_INSRT_CMD", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add("ins_cmd", OracleDbType.Varchar2, cmd, System.Data.ParameterDirection.Input);
            //com.Parameters.Add("ins_resp", OracleDbType.Clob, resp, System.Data.ParameterDirection.Input);
            com.Parameters.Add("CURS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);


            OracleCommand com2 = new OracleCommand("FLEX_GET_RESPONSE", con);
            com2.CommandType = System.Data.CommandType.StoredProcedure;

            int id = 0;

            string result = "";
            int isexecuted = 0;

            try
            {

                con.Open();
                id = Convert.ToInt32(com.ExecuteScalar().ToString());



                com2.Parameters.Add("ins_id", OracleDbType.Int32, id, System.Data.ParameterDirection.Input);
                com2.Parameters.Add("CURS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);



                bool isChecked = false;

                while (!isChecked)
                {
                    OracleDataReader dr = com2.ExecuteReader();
                    while (dr.Read())
                    {
                        result = dr["RESULT"].ToString();
                        isexecuted = Convert.ToInt32(dr["isexecuted"].ToString());
                    }
                    if (isexecuted == 1)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }

                }



            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }

            return result;

        }


    }
}
