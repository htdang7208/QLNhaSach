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
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.RECEIPTS.Where(x => x.isRemove == false)
                .Include(c => c.CUSTOMER)
                .Where(x => x.customerId == x.CUSTOMER.id)
                .Select(r => new CustomerReceiptInfo
                {
                    id = r.id,
                    firstName = r.CUSTOMER.firstName,
                    lastName = r.CUSTOMER.lastName,
                    phone = r.CUSTOMER.phone,
                    email = r.CUSTOMER.email,
                    address = r.CUSTOMER.address,
                    datePaid = r.datePaid,
                    payment = r.payment
                }).ToListAsync()
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.RECEIPTS.Where(x => x.isRemove == false && x.id == id)
                .Include(c => c.CUSTOMER)
                .Where(x => x.customerId == x.CUSTOMER.id)
                .Select(r => new CustomerReceiptInfo
                {
                    id = r.id,
                    firstName = r.CUSTOMER.firstName,
                    lastName = r.CUSTOMER.lastName,
                    phone = r.CUSTOMER.phone,
                    email = r.CUSTOMER.email,
                    address = r.CUSTOMER.address,
                    datePaid = r.datePaid,
                    payment = r.payment
                }).ToListAsync()
            };
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] CustomerReceiptInfo infoEnter)
        {
            RECEIPT receipt = new RECEIPT();
            if(string.IsNullOrEmpty(infoEnter.firstName) ||
                string.IsNullOrEmpty(infoEnter.lastName) ||
                string.IsNullOrEmpty(infoEnter.phone) ||
                string.IsNullOrEmpty(infoEnter.email) ||
                string.IsNullOrEmpty(infoEnter.address) ||
                infoEnter.datePaid == null ||
                infoEnter.payment == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Receipt,
                    Message = "Some fields is empty!"
                };
            }
            else
            {
                var customer = await _context.CUSTOMERS.Where(x => x.phone == infoEnter.phone).FirstOrDefaultAsync();
                // Khách hàng nãy đã tồn tại trong csdl
                if(customer != null && customer.isRemove == false)
                {
                    // Không được phép thu tiền vượt quá số tiền khách đang nợ
                    Roles policy = new Roles();
                    if(!policy.GetOverDept)
                    {
                        receipt.customerId = customer.id;
                        receipt.datePaid = infoEnter.datePaid;
                        if(infoEnter.payment > customer.dept)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.Get_Over_Dept,
                                Message = "Getting money is not over the money customer has been depted!"
                            };
                        }
                        else
                        {
                            receipt.payment = infoEnter.payment;
                            customer.dept -= infoEnter.payment;
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
                        receipt.customerId = customer.id;
                        receipt.datePaid = infoEnter.datePaid;
                        receipt.payment = infoEnter.payment;
                        customer.dept -= infoEnter.payment;
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
                // Thêm khách hàng mới
                else
                {
                    customer = new CUSTOMER();
                    customer.firstName = infoEnter.firstName;
                    customer.lastName = infoEnter.lastName;
                    customer.phone = infoEnter.phone;
                    customer.email = infoEnter.email;
                    customer.address = infoEnter.address;
                    customer.username = null;
                    customer.password = null;
                    customer.imageName = null;
                    customer.url = null;
                    customer.isRemove = false;
                    _context.CUSTOMERS.Add(customer);

                    receipt.datePaid = infoEnter.datePaid;
                    receipt.payment = infoEnter.payment;
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
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] CustomerReceiptInfo infoEnter)
        {
            RECEIPT receipt = await _context.RECEIPTS.Where(x => x.id == infoEnter.id && x.isRemove == false).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(infoEnter.firstName) ||
                string.IsNullOrEmpty(infoEnter.lastName) ||
                string.IsNullOrEmpty(infoEnter.phone) ||
                string.IsNullOrEmpty(infoEnter.email) ||
                string.IsNullOrEmpty(infoEnter.address) ||
                infoEnter.datePaid == null ||
                infoEnter.payment == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Receipt,
                    Message = "Some fields is empty!"
                };
            }
            else
            {
                var customer = await _context.CUSTOMERS.Where(x => x.phone == infoEnter.phone).FirstOrDefaultAsync();
                // Khách hàng nãy đã tồn tại trong csdl
                if (customer != null && customer.isRemove == false)
                {
                    // Không được phép thu tiền vượt quá số tiền khách đang nợ
                    Roles policy = new Roles();
                    if (!policy.GetOverDept)
                    {
                        receipt.customerId = customer.id;
                        receipt.datePaid = infoEnter.datePaid;
                        if (infoEnter.payment > customer.dept)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.Get_Over_Dept,
                                Message = "Getting money is not over the money customer has been depted!"
                            };
                        }
                        else
                        {
                            receipt.payment = infoEnter.payment;
                            customer.dept -= infoEnter.payment;
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
                        receipt.customerId = customer.id;
                        receipt.datePaid = infoEnter.datePaid;
                        receipt.payment = infoEnter.payment;
                        customer.dept -= infoEnter.payment;
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
                    return new BaseResponse
                    {
                        ErrorCode = Roles.NotFound,
                        Message = "Not find this customer!"
                    };
                }
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
    }
}