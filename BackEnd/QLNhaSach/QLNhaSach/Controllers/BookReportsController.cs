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
    public class BookReportsController : Controller
    {
        private readonly Context _context;
        public BookReportsController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            BookReport report = new BookReport();
            ListBookReport list = new ListBookReport();
            var books = await _context.BOOKS.ToListAsync();
            for(int i = 0; i < books.Count; i++)
            {
                var totalAmount = 0;
                var input = await _context.INPUTS.Where(x => x.bookId == books[i].id).ToListAsync();
                for (int j = 0; j < input.Count; j++)
                {
                    totalAmount += input[j].amount;
                }
                report.name = books[i].name;
                report.nowStock = books[i].stock;
                report.oldStock = books[i].stock - totalAmount;
                report.additionalStock = totalAmount;
                list.listBookReport.Add(report);
            }
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Completed create report!",
                Data = list.listBookReport
            };
        }
    }
}
