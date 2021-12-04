using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelParser.MyObject
{
    public class Group
    {
        public int weekNumber { get; set; }
        public string groupName { get; set; }
        public List<Para> paraList { get; set; }
    }
}
