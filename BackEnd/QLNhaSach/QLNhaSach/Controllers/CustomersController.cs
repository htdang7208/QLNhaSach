using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhaSach.Models;
using QLNhaSach.Models.Response;
using QLNhaSach.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QLNhaSach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public CustomersController(Context context, IHostingEnvironment ihostingEnvironment)
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
                Data = await _context.CUSTOMERS.Where(x => x.isRemove == false).ToListAsync()
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            var cus = await _context.CUSTOMERS.FindAsync(id);
            if (cus != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = cus
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound,
                Message = "Không tìm thấy khách hàng này trong CSDL!"
            };
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] CUSTOMER customer)
        {
            if (string.IsNullOrEmpty(customer.username) ||
                 string.IsNullOrEmpty(customer.password) ||
                 string.IsNullOrEmpty(customer.phone))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty,
                    Message = "Username\nPassword\nPhone\nKhông được phép bỏ trống!"
                };
            }
            // check exists
            var exists = await _context.CUSTOMERS
                .Where(cus => cus.username == customer.username)
                .FirstOrDefaultAsync();
            if(exists != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Existed_Username,
                    Message = "Username này đã tồn tại"
                };
            }
            exists = await _context.CUSTOMERS
                .Where(cus => cus.phone == customer.phone)
                .FirstOrDefaultAsync();
            if(exists != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Existed_Phone,
                    Message = "Phone này đã tồn tại"
                };
            }
            //update some fields client don't send
            customer.url = "";
            customer.isRemove = false;

            _context.CUSTOMERS.Add(customer);
            await _context.SaveChangesAsync();
            // change imageName, url
            var file = customer.file;
            if (file != null)
            {
                string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                string fileName = customer.id + "_" + customer.imageName;
                string path = _ihostingEnvironment.WebRootPath + "\\Customers\\" + fileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);

                    customer.imageName = fileName;
                    customer.url = domainUrl + "/Customers/" + fileName;  //http://localhost:59209/customers/back-end-ex-1.png

                    _context.Entry(customer).Property(x => x.imageName).IsModified = true;
                    _context.SaveChanges();
                }
            }
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Lưu thành công!",
                Data = customer
            };
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] CUSTOMER customer)
        {
            if (string.IsNullOrEmpty(customer.username) ||
                 string.IsNullOrEmpty(customer.password) ||
                 string.IsNullOrEmpty(customer.phone))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty,
                    Message = "username\npassword\nphone\nKhông được phép bỏ trống!"
                };
            }
            // check exists
            var exists = await _context.CUSTOMERS
                .Where(cus => cus.username == customer.username && cus.id != customer.id)
                .FirstOrDefaultAsync();
            if (exists != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Existed_Username,
                    Message = "Khách hàng với 'Tên tài khoản' này đã tồn tại"
                };
            }
            exists = await _context.CUSTOMERS
                .Where(cus => cus.phone == customer.phone && cus.id != customer.id)
                .FirstOrDefaultAsync();
            if (exists != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Existed_Phone,
                    Message = "Khách hàng với 'Số điện thoại' này đã tồn tại"
                };
            }
            var valid = await _context.CUSTOMERS.FindAsync(customer.id);

            valid.firstName = customer.firstName;
            valid.lastName = customer.lastName;
            valid.phone = customer.phone;
            valid.email = customer.email;
            valid.address = customer.address;
            valid.username = customer.username;
            valid.password = Helper.GenHash(customer.password);
            valid.oldDept = customer.oldDept;
            valid.nowDept = customer.nowDept;
            
            _context.CUSTOMERS.Update(valid);
            await _context.SaveChangesAsync();
            // change imageName, url
            var file = customer.file;
            if (file != null)
            {
                // delete old path
                string path = _ihostingEnvironment.WebRootPath + "\\Customers\\" + valid.imageName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                // add new path
                string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                string fileName = customer.id + "_" + customer.imageName;
                path = _ihostingEnvironment.WebRootPath + "\\Customers\\" + fileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);

                    customer.imageName = fileName;
                    customer.url = domainUrl + "/Customers/" + fileName;

                    _context.Entry(customer).Property(x => x.imageName).IsModified = true;
                    _context.SaveChanges();
                }
            }
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Cập nhật thành công!:",
                Data = customer
            };
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var cus = await _context.CUSTOMERS.FindAsync(id);
            cus.isRemove = true;
            _context.CUSTOMERS.Update(cus);
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Xóa thành công!"
            };
        }
    }
}