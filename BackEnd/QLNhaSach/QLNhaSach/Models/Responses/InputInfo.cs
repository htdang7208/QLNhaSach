using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class InputInfo
    {
        public int id { get; set; }
        public int stt { get; set; }
        public int bookId { get; set; }
        public string name { get; set; }
        public string kind { get; set; }
        public string author { get; set; }
        public int amount { get; set; }
        public bool isRemove { get; set; }
    }
}
