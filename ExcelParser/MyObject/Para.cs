using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelParser.MyObject
{
    public class Para
    {
        public string dayOfWeek { get; set; }
        public List<Time> timeList { get; set; }
        public int Number { get; set; }
        public Lesson lesson { get; set; }
    }
}
