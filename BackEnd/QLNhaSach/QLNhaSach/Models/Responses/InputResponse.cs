using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class InputResponse
    {
        public int id { get; set; }
        public List<InputDetailInfo> listInputInfo { get; set; }
        public bool isRemove { get; set; }
    }
}
