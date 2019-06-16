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
using QLNhaSach.Models.Responses;
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
        [HttpGet("getPhotoData/{Id}")]
        public ActionResult GetPhotoData(int Id)
        {
            ADMIN std = _context.ADMINS.Find(Id);
            if (std != null)
            {
                if (std.imageName.Length > 0)
                {
                    string path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + std.imageName;
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(path);
                        string base64Str = Convert.ToBase64String(bytes);

                        return Ok(new ImageInfo
                        {
                            FileName = std.imageName,
                            Extension = System.IO.Path.GetExtension(std.imageName),
                            Data = base64Str
                        });
                    }
                    catch { }
                }
            }
            return NoContent();
        }

        //Get Photourl of ADMIN
        [HttpGet("getPhotoUrl/{Id}")]
        public async Task<ActionResult<ADMIN>> GetPhotoUrl(int Id)
        {
            ADMIN std = await _context.ADMINS.FindAsync(Id);
            if (std != null)
            {
                if (std.imageName.Length > 0)
                {
                    string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                    string path = domainUrl + "/Admins/" + std.imageName;
                    return Ok(path);
                }
            }
            return NoContent();
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
        [HttpGet("adminRemoved")]
        public async Task<ActionResult<BaseResponse>> GetAdminRemoved()
        {
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.ADMINS.Where(x => x.isRemove == true).ToListAsync()
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
                if (admin.password != admin.confirmPassword)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Password_Not_Match_Confirm
                    };
                }
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
                    string fileName = admin.id + "_" + file.FileName;
                    string path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + fileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);

                        admin.imageName = fileName;
                        admin.url = domainUrl + "/Admins/" + fileName;  //http://localhost:59209/admins/back-end-ex-1.png

                        _context.Entry(admin).Property(x => x.imageName).IsModified = true;
                        _context.Entry(admin).Property(x => x.url).IsModified = true;
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
                ErrorCode = Roles.Empty
            };
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] ADMIN admin, int id)
        {
            if (string.IsNullOrEmpty(admin.username))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty
                };
            }
            var exists = await _context.ADMINS
                .Where(x => x.username == admin.username && x.id != id)
                .FirstOrDefaultAsync();
            if (exists != null)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Existed_Username
                };
            }
            var valid = await _context.ADMINS.FindAsync(id);

            valid.name = admin.name;
            valid.email = admin.email;
            valid.username = admin.username;

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
                string fileName = admin.id + "_" + file.FileName;
                path = _ihostingEnvironment.WebRootPath + "\\Admins\\" + fileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);

                    admin.imageName = fileName;
                    admin.url = domainUrl + "/Admins/" + fileName;

                    _context.Entry(admin).Property(x => x.imageName).IsModified = true;
                    _context.Entry(admin).Property(x => x.url).IsModified = true;
                    _context.SaveChanges();
                }
            }
            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }
        [HttpPut("changePassword/{id}")]
        public async Task<ActionResult<BaseResponse>> PutPassword(ChangePassword p, int id)
        {
            if (string.IsNullOrEmpty(p.oldPassword) ||
                string.IsNullOrEmpty(p.newPassword) ||
                string.IsNullOrEmpty(p.confirmPassword))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty
                };
            }
            var valid = await _context.ADMINS.Where(ad => ad.id == id).FirstOrDefaultAsync();
            string pass = Helper.GenHash(p.oldPassword);
            if (pass != valid.password)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Password_Not_Match_Origin
                };
            }
            if (p.newPassword != p.confirmPassword)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Password_Not_Match_Confirm
                };
            }

            valid.password = Helper.GenHash(valid.password);
            
            _context.ADMINS.Update(valid);
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }

        [HttpPut("restore/{id}")]
        public async Task<ActionResult<BaseResponse>> PutRestore(int id)
        {
            var valid = await _context.ADMINS.Where(ad => ad.id == id).FirstOrDefaultAsync();
            valid.isRemove = false;

            _context.ADMINS.Update(valid);
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success
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
                ErrorCode = Roles.NotFound
            };
        }
    }
}