using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class RoleInfo
    {
        public int MaxBookStock { get; set; }
        public int MinBookInput { get; set; }
        public bool GetOverDept { get; set; }
        public double DeptOver { get; set; }
        public int StockMax { get; set; }
    }
}
