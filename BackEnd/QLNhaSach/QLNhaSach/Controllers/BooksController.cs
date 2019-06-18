using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class BooksController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public BooksController(Context context, IHostingEnvironment ihostingEnvironment)
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
                Data = await _context.BOOKS.Where(x => x.isRemove == false).ToListAsync()
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            var exists = await _context.BOOKS.FindAsync(id);
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

        public string convertToUnicode(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        [HttpGet("search")]
        public async Task<ActionResult<BaseResponse>> Search([FromQuery] string q)
        {
            string name = convertToUnicode(q.Split("_")[0]);
            string kind = convertToUnicode(q.Split("_")[1]);

            if (name != "" && kind != "")
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = await _context.BOOKS
                    .Where(b => 
                    convertToUnicode(b.name) == name && 
                    convertToUnicode(b.kind) == kind).ToListAsync()
                };
            }
            else if (name != "") {
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = await _context.BOOKS
                    .Where(b => convertToUnicode(b.name) == name).ToListAsync()
                };
            }
            else if (kind != "")
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty,
                    Data = await _context
                    .BOOKS.Where(b => convertToUnicode(b.kind) == kind).ToListAsync()
                };
            }
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound
            };
        }

        [HttpGet("bookRemoved")]
        public async Task<ActionResult<BaseResponse>> GetBookRemoved()
        {
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.BOOKS.Where(x => x.isRemove == true).ToListAsync()
            };
        }
        
        public void Post(BOOK book)
        {
            book.state = true;
            book.url = "";
            book.isRemove = false;

            _context.BOOKS.Add(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] BOOK book, int id)
        {
            if (string.IsNullOrEmpty(book.name))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Name
                };
            } else if (string.IsNullOrEmpty(book.author))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Author
                };
            } else if (string.IsNullOrEmpty(book.kind))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Kind
                };
            } else if (book.price == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Price
                };
            } else if (book.stock == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Stock,
                    Message = "Empty book stock!"
                };
            } else {
                // check exists
                var exists = await _context.BOOKS.Where(b => b.name == book.name && b.id != id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Existed_Book
                    };
                }
                else
                {
                    // check policy
                    var policy = new Roles();
                    if (book.stock > policy.MaxBookStock)
                    {
                        return new BaseResponse
                        {
                            ErrorCode = Roles.OverflowMaxStock,
                            Message = policy.MaxBookStock + ""
                        };
                    }
                    else if (book.stock < policy.MinBookInput)
                    {
                        return new BaseResponse
                        {
                            ErrorCode = Roles.NotEnoughMinStock,
                            Message = policy.MinBookInput + ""
                        };
                    }

                    var valid = await _context.BOOKS.FindAsync(id);
                    valid.name = book.name;
                    valid.price = book.price;
                    valid.kind = book.kind;
                    valid.author = book.author;
                    valid.stock = book.stock;
                    _context.BOOKS.Update(valid);
                    await _context.SaveChangesAsync();
                    // change imageName, url
                    var file = book.file;
                    if (file != null)
                    {
                        // delete old path
                        string path = _ihostingEnvironment.WebRootPath + "\\Books\\" + valid.imageName;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        // add new path
                        string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                        string fileName = book.id + "_" + file.FileName;
                        path = _ihostingEnvironment.WebRootPath + "\\Books\\" + fileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);

                            book.imageName = fileName;
                            book.url = domainUrl + "/Books/" + fileName;

                            _context.Entry(book).Property(x => x.imageName).IsModified = true;
                            _context.Entry(book).Property(x => x.url).IsModified = true;
                            _context.SaveChanges();
                        }
                    }
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Success,
                        Data = book
                    };
                }
            }
        }

        [HttpPut("restore/{id}")]
        public async Task<ActionResult<BaseResponse>> PutRestore(int id)
        {
            var valid = await _context.BOOKS.Where(b => b.id == id).FirstOrDefaultAsync();
            valid.isRemove = false;

            _context.BOOKS.Update(valid);
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var exists = await _context.BOOKS.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.BOOKS.Update(exists);
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