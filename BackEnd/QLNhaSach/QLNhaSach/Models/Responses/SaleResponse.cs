using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models.Responses
{
    public class SaleResponse
    {
        public int saleId { get; set; }
        public int customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateCreated { get; set; }
        public List<SaleDetailInfo> listSaleDetailInfo { get; set; }        
    }
}
