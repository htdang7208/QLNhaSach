using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class InputsController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment _ihostingEnvironment;
        public InputsController(Context context, IHostingEnvironment ihostingEnvironment)
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
                Data = await _context.INPUTS
                .Include(b=>b.BOOK)
                .Where(x => x.isRemove == false && x.bookId == x.BOOK.id)
                .Select(s => new InputInfo
                {
                    id = s.id,
                    stt = s.stt,
                    bookId = s.BOOK.id,
                    name = s.BOOK.name,
                    kind = s.BOOK.kind,
                    author = s.BOOK.author,
                    amount = s.amount,
                    isRemove = s.isRemove
                }).ToListAsync()
            };
        }

        [HttpGet("{stt}")]
        public async Task<ActionResult<BaseResponse>> Get(int stt)
        {
            var list = await _context.INPUTS.Include(b => b.BOOK)
                .Where(x => x.isRemove == false && x.bookId == x.BOOK.id && x.stt == stt)
                .Select(s => new InputInfo
                {
                    id = s.id,
                    stt = s.stt,
                    bookId = s.BOOK.id,
                    name = s.BOOK.name,
                    kind = s.BOOK.kind,
                    author = s.BOOK.author,
                    amount = s.amount,
                    isRemove = s.isRemove
                }).ToListAsync();
           
            return new BaseResponse
            {
                ErrorCode = Roles.NotFound,
                Data = list
            };
        }

        [HttpGet("inputRemoved")]
        public async Task<ActionResult<BaseResponse>> GetInputRemoved()
        {
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = await _context.INPUTS.Where(x => x.isRemove == true).ToListAsync()
            };
        }

        public string convertToUnicode(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Post(ListInputInfo listInput)
        {
            var list = listInput.listInputInfo;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].name) ||
                 string.IsNullOrEmpty(list[i].kind) ||
                 string.IsNullOrEmpty(list[i].author) ||
                 list[i].amount == 0)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Empty
                    };
                }
                
                var exists = await _context.BOOKS
                    .Where(bo => convertToUnicode(bo.name) == convertToUnicode(list[i].name) &&
                    bo.id != list[i].bookId).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Existed_Book
                    };
                }
                // check policy
                Roles policy = new Roles();
                if (list[i].amount < policy.MinBookInput)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.NotEnoughMinStock,
                        Message = policy.MinBookInput + ""
                    };
                }
                BOOK b = new BOOK();
                b.name = list[i].name;
                b.kind = list[i].kind;
                b.author = list[i].author;
                b.price = 0;
                b.stock = list[i].amount;
                b.imageName = "";
                b.url = "";
                b.isRemove = false;
                _context.BOOKS.Add(b);
                _context.SaveChanges();

                INPUT input = new INPUT();
                input.stt = list[i].stt;
                input.bookId = list[i].bookId;
                input.amount = list[i].amount;
                input.isRemove = false;
                _context.INPUTS.Add(input);
                _context.SaveChanges();
            }

            return new BaseResponse { ErrorCode = Roles.Success };
            
        }

        [HttpPut("{stt}")]
        public async Task<ActionResult<BaseResponse>> Put(ListInputInfo listInput, int stt)
        {
            //var valid = await _context.INPUTS.Include(b => b.BOOK)
            //    .Where(x => x.isRemove == false && x.bookId == x.BOOK.id && x.stt == stt)
            //    .Select(s => new InputInfo
            //    {
            //        id = s.id,
            //        stt = s.stt,
            //        bookId = s.BOOK.id,
            //        name = s.BOOK.name,
            //        kind = s.BOOK.kind,
            //        author = s.BOOK.author,
            //        amount = s.amount,
            //        isRemove = s.isRemove
            //    }).ToListAsync();

            var list = listInput.listInputInfo;
            //if (valid.Count != list.Count)
            //{
            //    return new BaseResponse { ErrorCode = Roles.NotFound };
            //}

            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i].name) ||
                 string.IsNullOrEmpty(list[i].kind) ||
                 string.IsNullOrEmpty(list[i].author) ||
                 list[i].amount == 0)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Empty
                    };
                }

                var book = await _context.BOOKS
                    .Where(bo => convertToUnicode(bo.name) == convertToUnicode(list[i].name) &&
                    bo.id != list[i].bookId).FirstOrDefaultAsync();
                if (book != null)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Existed_Book
                    };
                }
                // check policy
                book = await _context.BOOKS.FindAsync(list[i].bookId);
                var policy = new Roles();
                if (book.stock > policy.MaxBookStock)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.OverflowMaxStock,
                        Message = policy.MaxBookStock + ""
                    };
                }
                else if (list[i].amount < policy.MinBookInput)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.NotEnoughMinStock,
                        Message = policy.MinBookInput + ""
                    };
                }
                book.name = list[i].name;
                book.kind = list[i].kind;
                book.author = list[i].author;
                book.stock += list[i].amount;
                _context.BOOKS.Update(book);
                _context.SaveChanges();

                INPUT input = new INPUT();
                input.stt = list[i].stt;
                input.bookId = list[i].bookId;
                input.amount = list[i].amount;
                input.isRemove = false;
                _context.INPUTS.Update(input);
                _context.SaveChanges();
            }

            return new BaseResponse { ErrorCode = Roles.Success };
        }

        [HttpPut("restore/{stt}")]
        public async Task<ActionResult<BaseResponse>> PutRestore(int stt)
        {
            var list = await _context.INPUTS.Where(i => i.stt == stt).ToListAsync();
            for(int i = 0; i < list.Count; i++)
            {
                list[i].isRemove = false;
                _context.INPUTS.Update(list[i]);
                _context.SaveChanges();
            }

            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }

        [HttpDelete("{stt}")]
        public async Task<ActionResult<BaseResponse>> Delete(int stt)
        {
            var list = await _context.INPUTS.Where(i => i.stt == stt).ToListAsync();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].isRemove = true;
                _context.INPUTS.Update(list[i]);
                _context.SaveChanges();
            }

            return new BaseResponse
            {
                ErrorCode = Roles.Success
            };
        }
    }
}