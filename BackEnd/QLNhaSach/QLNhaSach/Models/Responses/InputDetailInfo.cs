using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class InputDetailInfo
    {
        public int stt { get; set; }
        public string name { get; set; }
        public string kind { get; set; }
        public string author { get; set; }
        public int amount { get; set; }
        public int inputId { get; set; }
    }
}
