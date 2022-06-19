using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUD_FLEX_API
{
    class Commands
    {
        public string command { get; set; }
        public int id { get; set; }
        public int status { get; set; }
        
        public bool isCorrect()
        {
            if (command.Contains("update") || command.Contains("insert") || command.Contains("delete"))
            {
                return false;
            }
            else
                return true;
        }
    }
}
