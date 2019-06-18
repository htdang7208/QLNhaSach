using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaSach.Models;
using QLNhaSach.Models.Response;
using QLNhaSach.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QLNhaSach.Controllers
{
    [Route("api/[controller]")]
    public class DeptReportsController : Controller
    {
        private readonly Context _context;
        public DeptReportsController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            List<DeptReport> list = new List<DeptReport>();
            var customers = await _context.CUSTOMERS.ToListAsync();
            for (int i = 0; i < customers.Count; i++)
            {
                var receipt = await _context.RECEIPTS.Where(re => re.customerId == customers[i].id).FirstOrDefaultAsync();
                double res = Math.Abs(receipt.customerPaid - receipt.total);
                DeptReport report = new DeptReport();
                report.name = customers[i].lastName + " " + customers[i].firstName;
                report.nowDept = customers[i].nowDept;
                report.additionalDept = res;
                report.oldDept = customers[i].oldDept;
                list.Add(report);
            }
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Completed create report!",
                Data = list
            };
        }
    }
}
