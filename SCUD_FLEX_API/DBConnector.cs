using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    class DBConnector
    {
        public static List<Commands> getUnexecutedCOmmands()
        {

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
            OracleCommand com = new OracleCommand("GET_FLEX_UNEXECUTED_CMDS", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            //com.Parameters.Add("insertion_type", OracleDbType.Int32, insertType, System.Data.ParameterDirection.Input);
            com.Parameters.Add("CURS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);


            List<Commands> lis = new List<Commands>();

            try
            {
                con.Open();
                OracleDataReader dr= com.ExecuteReader();

                while (dr.Read())
                {
                    Commands cmnd = new Commands();
                    cmnd.command=dr["COMMAND"].ToString();
                    cmnd.id = Convert.ToInt32(dr["ID"].ToString());
                    lis.Add(cmnd);
                }

            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }

            return lis;

        }

        public static string executeCommand(string command)
        {
            OleDbConnection conect = new OleDbConnection(ConfigurationManager.AppSettings["dbFile"]);
            string dtn = DateTime.Now.ToString("dd.MM.yyyy");
            //dtn = "03.02.2020";
            //string kv = ConfigurationManager.AppSettings["keyV"];
           // string cmd = @"SELECT EVENTS.*, USERS.* FROM (USERS INNER JOIN EVENTS ON USERS.SID = EVENTS.USER_SID) WHERE (dateValue(EVENTS.EVENT_TIME) = dateValue('" + dtn + "')) AND (USERS.KEY_NUMBER = " + kv + ") and EVENTS.ID>" + lastId + "";

            //List<AccessInformation> lis = new List<AccessInformation>();

            OleDbCommand com = new OleDbCommand(command, conect);

            string response = "";

            try
            {

                conect.Open();
                OleDbDataReader dr = com.ExecuteReader();


                List<List<ItemModel>> fulObj = new List<List<ItemModel>>();

                List<ItemModel> curObj = new List<ItemModel>();

                while (dr.Read())
                {
                    string per = "";
                    ItemModel md = new ItemModel();
                    curObj = new List<ItemModel>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        md = new ItemModel();
                        md.value = dr[i].ToString();
                        md.name = dr.GetName(i);

                        curObj.Add(md);
                    }
                    fulObj.Add(curObj);

                }



                response = fulObj.getJsObjList();
            }
            catch(Exception ex)
            {
                response = ex.Message;
            }
            finally
            {
                conect.Close();
            }

            return response;
        }

        public static bool fixResponse(string resp,int id,int status)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
            OracleCommand com = new OracleCommand("FLEX_UPDT_REQ", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add("Ins_id", OracleDbType.Int32, id, System.Data.ParameterDirection.Input);
            com.Parameters.Add("ins_resp", OracleDbType.Clob, resp, System.Data.ParameterDirection.Input);
            com.Parameters.Add("ins_status", OracleDbType.Int32, status, System.Data.ParameterDirection.Input);
            //com.Parameters.Add("CURS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);


            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }

            return false;

        }


        public static void testConnect()
        {
            OleDbConnection conect = new OleDbConnection(ConfigurationManager.AppSettings["dbFile"]);

            try
            {
                conect.Open();
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["dir"] + "\\Dbexcpt.txt"))
                {
                    sw.Write(ex.Message);
                }
            }
            finally
            {
                conect.Close();
            }

        }

    }


}
