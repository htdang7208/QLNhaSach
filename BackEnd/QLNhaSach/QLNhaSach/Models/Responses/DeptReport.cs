using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    public class DeptReport
    {
        public string name { get; set; }
        public double oldDept { get; set; }
        public double nowDept { get; set; }
        public double additionalDept { get; set; }
    }
    //public class ListDeptReport
    //{
    //    public List<DeptReport> listDeptReport { get; set; }
    //}
}
