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
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post([FromForm] BOOK book)
        {
            if (string.IsNullOrEmpty(book.name))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Name,
                    Message = "Empty book name!"
                };
            } else if (string.IsNullOrEmpty(book.author))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Author,
                    Message = "Empty book author!"
                };
            } else if (string.IsNullOrEmpty(book.kind))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Kind,
                    Message = "Empty book kind!"
                };
            } else if(book.price == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Price,
                    Message = "Empty book price!"
                };
            } else if(book.stock == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Stock,
                    Message = "Empty book stock!"
                };
            } else {
                // check exists
                var policy = new Roles();
                var exists = await _context.BOOKS.Where(x => x.name == book.name).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Existed_Book_Name,
                        Message = "Book's name has already existed. Please enter different book's name!",
                    };
                }
                // check policy
                else if (exists.stock > policy.MaxBookStock)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.OverflowMaxStock,
                        Message = "Maximum stock: " + policy.MaxBookStock + ". You cannot input!"
                    };
                } else if (book.stock <= policy.MinBookStock)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.NotEnoughMinStock,
                        Message = "Please input minimum " + policy.MinBookStock + " books!"
                    };
                }
                //update some fields:
                book.stock += exists.stock;
                book.state = true;
                book.url = "";
                book.isRemove = false;
                // add object to database
                _context.BOOKS.Add(book);
                await _context.SaveChangesAsync();
                // change imageName, url
                var file = book.file;
                if (file != null)
                {
                    string domainUrl = Request.Scheme + "://" + Request.Host.ToString();
                    string fileName = book.id + "_" + book.imageName;
                    string path = _ihostingEnvironment.WebRootPath + "\\Books\\" + fileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);

                        book.imageName = fileName;
                        book.url = domainUrl + "/Books/" + fileName;  //http://localhost:59209/books/back-end-ex-1.png

                        _context.Entry(book).Property(x => x.imageName).IsModified = true;
                        _context.SaveChanges();
                    }
                }
                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Message = "Book created successfully!",
                    Data = book
                };
            }
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] BOOK book)
        {
            if (string.IsNullOrEmpty(book.name))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Name,
                    Message = "Empty book name!"
                };
            } else if (string.IsNullOrEmpty(book.author))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Author,
                    Message = "Empty book author!"
                };
            } else if (string.IsNullOrEmpty(book.kind))
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Kind,
                    Message = "Empty book kind!"
                };
            } else if (book.price == 0)
            {
                return new BaseResponse
                {
                    ErrorCode = Roles.Empty_Book_Price,
                    Message = "Empty book price!"
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
                var exists = await _context.BOOKS
                    .Where(x => x.name == book.name && x.id != book.id)
                    .FirstOrDefaultAsync();
                if (exists != null)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Existed_Book_Name,
                        Message = "Book's name has already existed. Please enter different book's name!",
                        Data = null
                    };
                }
                else {
                    var valid = await _context.BOOKS.FindAsync(book.id);

                    valid.name = book.name;
                    valid.price = book.price;
                    valid.kind = book.kind;
                    valid.author = book.author;
                    //valid.stock += book.stock;
                    // add object to database
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
                        string fileName = book.id + "_" + book.imageName;
                        path = _ihostingEnvironment.WebRootPath + "\\Books\\" + fileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);

                            book.imageName = fileName;
                            book.url = domainUrl + "/Books/" + fileName;

                            _context.Entry(book).Property(x => x.imageName).IsModified = true;
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