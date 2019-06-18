using System;
using System.Collections.Generic;
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
    public class ReceiptsController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public ReceiptsController(Context context, IHostingEnvironment ihostingEnvironment)
        {
            _context = context;
            _ihostingEnvironment = ihostingEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            var re = await _context.RECEIPTS.Where(x => x.isRemove == false)
                .Include(c => c.CUSTOMER)
                .Where(x => x.customerId == x.CUSTOMER.id)
                .Select(r => new CustomerReceiptInfo
                {
                    id = r.id,
                    customerId = r.customerId,
                    firstName = r.CUSTOMER.firstName,
                    lastName = r.CUSTOMER.lastName,
                    phone = r.CUSTOMER.phone,
                    email = r.CUSTOMER.email,
                    address = r.CUSTOMER.address,
                    dateCreated = r.dateCreated,
                    customerPaid = r.customerPaid,
                    total = r.total
                }).ToListAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = re
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.RECEIPTS.Where(re => re.isRemove == false && re.id == id)
                .Include(c => c.CUSTOMER)
                .Where(re => re.customerId == re.CUSTOMER.id)
                .Select(re => new CustomerReceiptInfo
                {
                    id = re.id,
                    firstName = re.CUSTOMER.firstName,
                    lastName = re.CUSTOMER.lastName,
                    phone = re.CUSTOMER.phone,
                    email = re.CUSTOMER.email,
                    address = re.CUSTOMER.address,
                    dateCreated = re.dateCreated,
                    customerPaid = re.customerPaid,
                    total = re.total
                }).FirstOrDefaultAsync()
            };
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] CustomerReceiptInfo data)
        {
            RECEIPT receipt = new RECEIPT();
            var customer = await _context.CUSTOMERS.Where(cus => cus.phone == data.phone).FirstOrDefaultAsync();
            if (customer != null && customer.isRemove == false)
            {
                // Không được phép thu tiền vượt quá số tiền khách đang nợ
                if (!Roles.GetOverDept)
                {
                    if (data.customerPaid > customer.oldDept)
                    {
                        return new BaseResponse
                        {
                            ErrorCode = Roles.Get_Over_Dept,
                            Message = "Getting money is not over the money customer has been depted!"
                        };
                    }
                    else
                    {
                        double res = data.customerPaid - data.total;

                        receipt.customerId = customer.id;
                        receipt.dateCreated = data.dateCreated;
                        receipt.total = data.total;
                        receipt.customerPaid = data.customerPaid;
                        customer.nowDept = res > 0 ? Math.Abs(res - customer.oldDept) : customer.oldDept + Math.Abs(res);
                        _context.CUSTOMERS.Update(customer);
                        _context.RECEIPTS.Add(receipt);
                        await _context.SaveChangesAsync();
                        return new BaseResponse
                        {
                            ErrorCode = Roles.Success,
                            Message = "A receipt has just created!"
                        };
                    }
                }
                else
                {
                    double res = data.customerPaid - data.total;

                    receipt.customerId = customer.id;
                    receipt.dateCreated = data.dateCreated;
                    receipt.total = data.total;
                    receipt.customerPaid = data.customerPaid;
                    customer.nowDept = res > 0 ? Math.Abs(res - customer.oldDept) : customer.oldDept + Math.Abs(res);
                    _context.CUSTOMERS.Update(customer);
                    _context.RECEIPTS.Add(receipt);
                    await _context.SaveChangesAsync();
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Success,
                        Message = "A receipt has just created!"
                    };
                }
            }
            // Khách hàng này chưa có trong csdl
            else
            {
                customer = new CUSTOMER();
                customer.firstName = data.firstName;
                customer.lastName = data.lastName;
                customer.phone = data.phone;
                customer.email = data.email;
                customer.address = data.address;
                customer.username = null;
                customer.password = null;
                customer.imageName = null;
                customer.url = null;
                customer.oldDept = 0;
                customer.nowDept = 0;
                customer.isRemove = false;
                _context.CUSTOMERS.Add(customer);

                receipt.dateCreated = data.dateCreated;
                receipt.total = data.total;
                receipt.isRemove = false;
                _context.RECEIPTS.Add(receipt);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Message = "A receipt has just created!"
                };
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] CustomerReceiptInfo data, int id)
        {
            if(data.total == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Customer_Total
                };
            }
            if (data.customerPaid == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Customer_Paid
                };
            }
            var receipt = await _context.RECEIPTS.Where(re => re.id == data.id).FirstOrDefaultAsync();
            var customer = await _context.CUSTOMERS.Where(cus => cus.id == data.customerId).FirstOrDefaultAsync();
            // Không được phép thu tiền vượt quá số tiền khách đang nợ
            if (!Roles.GetOverDept)
            {
                if (data.customerPaid > customer.oldDept)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Get_Over_Dept,
                        Message = "Getting money is not over the money customer has been depted!"
                    };
                }
                else
                {
                    double res = data.customerPaid - data.total;

                    receipt.customerId = customer.id;
                    receipt.dateCreated = data.dateCreated;
                    receipt.total = data.total;
                    receipt.customerPaid = data.customerPaid;
                    customer.nowDept = res > 0 ? Math.Abs(res - customer.oldDept) : customer.oldDept + Math.Abs(res);
                    _context.CUSTOMERS.Update(customer);
                    _context.RECEIPTS.Add(receipt);
                    await _context.SaveChangesAsync();
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Success,
                        Message = "A receipt has just updated!"
                    };
                }
            }
            else
            {
                double res = data.customerPaid - data.total;

                receipt.customerId = customer.id;
                receipt.dateCreated = data.dateCreated;
                receipt.total = data.total;
                receipt.customerPaid = data.customerPaid;
                customer.nowDept = res > 0 ? Math.Abs(res - customer.oldDept) : customer.oldDept + Math.Abs(res);
                _context.CUSTOMERS.Update(customer);
                _context.RECEIPTS.Add(receipt);
                await _context.SaveChangesAsync();
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Message = "A receipt has just updated!"
                };
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var exists = await _context.RECEIPTS.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.RECEIPTS.Update(exists);
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

        [HttpGet("receiptRemoved")]
        public async Task<ActionResult<BaseResponse>> GetCustomerRemoved()
        {
            var re = await _context.RECEIPTS.Where(x => x.isRemove == true)
                .Include(c => c.CUSTOMER)
                .Where(x => x.customerId == x.CUSTOMER.id)
                .Select(r => new CustomerReceiptInfo
                {
                    id = r.id,
                    customerId = r.customerId,
                    firstName = r.CUSTOMER.firstName,
                    lastName = r.CUSTOMER.lastName,
                    phone = r.CUSTOMER.phone,
                    email = r.CUSTOMER.email,
                    address = r.CUSTOMER.address,
                    dateCreated = r.dateCreated,
                    customerPaid = r.customerPaid,
                    total = r.total
                }).ToListAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = re
            };
        }

        [HttpPut("restore/{id}")]
        public async Task<ActionResult<BaseResponse>> PutRestore(int id)
        {
            var valid = await _context.RECEIPTS.Where(cus => cus.id == id).FirstOrDefaultAsync();
            valid.isRemove = false;

            _context.RECEIPTS.Update(valid);
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }

    }
}