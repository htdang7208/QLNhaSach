using System;
using System.Collections.Generic;
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
    public class AdminsController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public AdminsController(Context context, IHostingEnvironment ihostingEnvironment)
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
                Data = await _context.ADMINS.Where(x => x.isRemove == false).ToListAsync()
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            var exists = await _context.ADMINS.FindAsync(id);
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
        public async Task<ActionResult<BaseResponse>> Post([FromForm] ADMIN admin)
        {
            if (!string.IsNullOrEmpty(admin.username) ||
                 !string.IsNullOrEmpty(admin.password))
            {
                //update some fields:
                admin.url = "";
                admin.isRemove = false;
                // add object to database
                _context.ADMINS.Add(admin);
                await _context.SaveChangesAsync();
                // change imageName, url
                var file = admin.file;
                if (file != null)
                {
                    string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                    string fileName = admin.id + "_" + admin.imageName;
                    string path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + fileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);

                        admin.imageName = fileName;
                        admin.url = domainUrl + "/Admins/" + fileName;  //http://localhost:59209/admins/back-end-ex-1.png

                        _context.Entry(admin).Property(x => x.imageName).IsModified = true;
                        _context.SaveChanges();
                    }
                }
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = admin
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
        public async Task<ActionResult<BaseResponse>> Put([FromForm] ADMIN admin)
        {
            if (!string.IsNullOrEmpty(admin.username) ||
                !string.IsNullOrEmpty(admin.password))
            {
                var exists = await _context.ADMINS
                    .Where(x => x.username == admin.username && x.id != admin.id)
                    .FirstOrDefaultAsync();
                if (exists != null)
                {
                    var valid = await _context.ADMINS.FindAsync(admin.id);

                    valid.name = admin.name;
                    valid.email = admin.email;
                    valid.username = admin.username;
                    valid.password = Helper.GenHash(admin.password);

                    // add object to database
                    _context.ADMINS.Update(valid);
                    await _context.SaveChangesAsync();
                    // change imageName, url
                    var file = admin.file;
                    if (file != null)
                    {
                        // delete old path
                        string path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + valid.imageName;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        // add new path
                        string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                        string fileName = admin.id + "_" + admin.imageName;
                        path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + fileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);

                            admin.imageName = fileName;
                            admin.url = domainUrl + "/Admins/" + fileName;

                            _context.Entry(admin).Property(x => x.imageName).IsModified = true;
                            _context.SaveChanges();
                        }
                    }
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Success,
                        Data = admin
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
            var exists = await _context.ADMINS.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.ADMINS.Update(exists);
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