using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class CustomerReceiptInfo
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateTime datePaid { get; set; }
        public double payment { get; set; }
    }
}
