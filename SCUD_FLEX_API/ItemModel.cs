using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    class ItemModel
    {
        public string name { get; set; }
        public string value { get; set; }


        public string getField()
        {
            if (value != "")
            {

                return "\"" + name + "\": \"" + value + "\"";
            }
            else
            {
                return "\"" + name + "\": null";
            }
        }

    }
}
