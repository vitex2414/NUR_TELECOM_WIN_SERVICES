using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    static class CustomExtension
    {

        public static string getJsObj(this List<ItemModel> lis)
        {
            string obj = "";

            if (lis.Count > 0)
                obj = "{";

            for (int i = 0; i < lis.Count; i++)
            {

                obj += lis[i].getField();

                if (i < lis.Count - 1)
                {
                    obj += ',';
                }
                else
                {
                    obj += '}';
                }

            }

            return obj;
        }

        public static string getJsObjList(this List<List<ItemModel>> lis)
        {
            string obj = "";


            if (lis.Count > 0)
                obj = "[";
            else
            {
                obj = "[]";
            }

            for (int i = 0; i < lis.Count; i++)
            {
                obj += lis[i].getJsObj();
                if (i < lis.Count - 1)
                {
                    obj += ',';
                }
                else
                {
                    obj += ']';
                }
            }


            return obj;

        }

    }
}
