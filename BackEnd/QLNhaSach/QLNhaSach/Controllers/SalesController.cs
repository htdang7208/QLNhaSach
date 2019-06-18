using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaSach.Models;
using QLNhaSach.Models.Response;
using QLNhaSach.Models.Responses;
using QLNhaSach.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QLNhaSach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public SalesController(Context context, IHostingEnvironment ihostingEnvironment)
        {
            _context = context;
            _ihostingEnvironment = ihostingEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            // danh sách chi tiết từng hóa đơn - tổng hợp, trộn lẫn
            var detail = await _context.SALEDETAILS
                .Include(s => s.SALE).Include(b => b.BOOK)
                .Where(x => x.saleId == x.SALE.id && x.SALE.isRemove == false && x.bookId == x.BOOK.id)
                .Select(i => new SaleDetailInfo
                {
                    stt = i.stt,
                    bookId = i.bookId,
                    name = i.BOOK.name,
                    kind = i.BOOK.kind,
                    price = i.BOOK.price,
                    amount = i.amount,
                    saleId = i.SALE.id
                }).ToListAsync();

            // phân loại lại theo từng id của hóa đơn
            var listSale = await _context.SALES.Where(x => x.isRemove == false)
                .Select(i => new SaleResponse
                {
                    saleId = i.id,
                    customerId = i.CUSTOMER.id,
                    firstName = i.CUSTOMER.firstName,
                    lastName = i.CUSTOMER.lastName,
                    listSaleDetailInfo = detail.FindAll(x => x.saleId == i.id)
                }).ToListAsync();

            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = listSale
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            var exists = await _context.SALES.FindAsync(id);
            if (exists != null && exists.isRemove)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Removed,
                    Message = "Is removed!"
                };
            }
            else if (exists != null)
            {
                var detail = await _context.SALEDETAILS
                .Include(i => i.SALE).Include(b => b.BOOK)
                .Where(x => x.saleId == x.SALE.id && x.SALE.isRemove == false && x.bookId == x.BOOK.id)
                .Select(i => new SaleDetailInfo
                {
                    stt = i.stt,
                    bookId = i.bookId,
                    name = i.BOOK.name,
                    kind = i.BOOK.kind,
                    price = i.BOOK.price,
                    amount = i.amount,
                    saleId = i.SALE.id
                }).ToListAsync();

                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = await _context.SALES.Where(x => x.isRemove == false && x.id == id)
                    .Select(i => new SaleResponse
                    {
                        saleId = i.id,
                        customerId = i.CUSTOMER.id,
                        firstName = i.CUSTOMER.firstName,
                        lastName = i.CUSTOMER.lastName,
                        listSaleDetailInfo = detail.FindAll(x => x.saleId == i.id)
                    }).FirstOrDefaultAsync()
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound,
                Message = "Not found!"
            };
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] SaleResponse infoEnter)
        {
            // Thêm một hóa đơn mới
            SALE sale = new SALE();
            sale.customerId = infoEnter.customerId;
            sale.dateCreated = infoEnter.dateCreated;
            sale.isRemove = false;
            _context.SALES.Add(sale);
            await _context.SaveChangesAsync();

            // Kiểm tra đã nhập thông tin khách hàng - Tên, Ngày lập hóa đơn
            if (string.IsNullOrEmpty(infoEnter.firstName) ||
                string.IsNullOrEmpty(infoEnter.lastName) ||
                infoEnter.dateCreated == null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Customer_Input,
                    Message = "Customer info is Empty!"
                };
            }

            // Cập nhật phiếu nhập mới - duyệt từng thông tin sách (Tên, Thể loại, Số lượng, Đơn giá)
            var list = infoEnter.listSaleDetailInfo;
            for (int i = 0; i < list.Count; i++)
            {
                // Kiểm tra dữ liệu nhập bị trống
                if (string.IsNullOrEmpty(list[i].name) ||
                    string.IsNullOrEmpty(list[i].kind) ||
                    list[i].amount == 0 ||
                    list[i].price == 0)
                {
                    return new BaseResponse
                    {
                        //ErrorCode = Roles.Empty_Book_Input,
                        Message = "Some field is empty!",
                        Data = null
                    };
                }
                else
                {
                    SALEDETAIL saleDetail = new SALEDETAIL();
                    var bookNeed = await _context.BOOKS.Where(x => x.name == list[i].name).FirstOrDefaultAsync();
                    saleDetail.stt = i + 1;
                    saleDetail.bookId = bookNeed.id;
                    saleDetail.saleId = list[i].saleId; // Nối saleDetail và sale lại với nhau
                    saleDetail.amount = list[i].amount;
                    saleDetail.totalPrice = list[i].price * list[i].amount;

                    _context.SALEDETAILS.Add(saleDetail);
                    await _context.SaveChangesAsync();
                }
            }

            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "A book sale bill is created!"
            };
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] SaleResponse infoEnter)
        {
            // Cập nhật phiếu nhập mới
            var list = infoEnter.listSaleDetailInfo;
            for (int i = 0; i < list.Count; i++)
            {
                // Kiểm tra dữ liệu nhập bị trống
                if (string.IsNullOrEmpty(list[i].name) ||
                    string.IsNullOrEmpty(list[i].kind) ||
                    list[i].amount == 0 ||
                    list[i].price == 0)
                {
                    return new BaseResponse
                    {
                        //ErrorCode = Roles.Empty_Book_Input,
                        Message = "Some field is empty!",
                        Data = null
                    };
                }
                else
                {
                    var saleDetail = await _context.SALEDETAILS.Where(x => x.saleId == infoEnter.saleId && x.stt == list[i].stt).FirstOrDefaultAsync();
                    if(saleDetail == null)
                    {
                        return new BaseResponse
                        {
                            ErrorCode = Roles.NotFound,
                            Message = "Book's name: " + list[i].name + " is not found!"
                        };
                    }
                    else
                    {
                        saleDetail.bookId = list[i].bookId;
                        saleDetail.amount = list[i].amount;
                        saleDetail.totalPrice = list[i].price * list[i].amount;

                        _context.SALEDETAILS.Update(saleDetail);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Book sale bill is update!"
            };
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var exists = await _context.SALES.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.SALES.Update(exists);
                await _context.SaveChangesAsync();
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Message = "Deleted!"
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound,
                Message = "Not found!"
            };
        }
    }
}