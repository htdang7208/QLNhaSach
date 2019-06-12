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
            var exists = await _context.CUSTOMERS.FindAsync(id);
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
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = exists
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound,
                Message = "Not found!"
            };
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] CUSTOMER customer)
        {
            if (!string.IsNullOrEmpty(customer.username) ||
                 !string.IsNullOrEmpty(customer.password))
            {
                //update some fields:
                customer.url = "";
                customer.isRemove = false;
                // add object to database
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
                    Data = customer
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.EmptyUsernamePassword,
                Message = "Username of Password is empty!",
                Data = null
            };
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] CUSTOMER customer)
        {
            if (!string.IsNullOrEmpty(customer.username) ||
                !string.IsNullOrEmpty(customer.password))
            {
                var exists = await _context.CUSTOMERS
                    .Where(x => x.username == customer.username && x.id != customer.id)
                    .FirstOrDefaultAsync();
                if (exists != null)
                {
                    var valid = await _context.CUSTOMERS.FindAsync(customer.id);

                    valid.firstName = customer.firstName;
                    valid.lastName = customer.lastName;
                    valid.phone = customer.phone;
                    valid.email = customer.email;
                    valid.address = customer.address;
                    valid.username = customer.username;
                    valid.password = Helper.GenHash(customer.password);
                    valid.dept = customer.dept;

                    // add object to database
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
                        Data = customer
                    };
                }
                return new BaseResponse
                {
                    ErrorCode = Roles.ExistedUsername,
                    Message = "Username has existed!",
                    Data = null
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.EmptyUsernamePassword,
                Message = "Username of Password is empty!",
                Data = null
            };
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var exists = await _context.CUSTOMERS.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.CUSTOMERS.Update(exists);
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