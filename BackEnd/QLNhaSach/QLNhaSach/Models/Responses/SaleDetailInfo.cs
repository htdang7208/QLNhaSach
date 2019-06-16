using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class SaleDetailInfo
    {
        public int stt { get; set; }
        public int bookId { get; set; }
        public int saleId { get; set; }
        public string name { get; set; }
        public string kind { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
    }
}
