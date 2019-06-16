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
            var detail = await _context.INPUTDETAILS
                .Include(i => i.INPUT).Include(b => b.BOOK)
                .Where(x => x.inputId == x.INPUT.id && x.INPUT.isRemove == false && x.bookId == x.BOOK.id)
                .Select(i => new InputDetailInfo
                {
                    stt = i.stt,
                    name = i.BOOK.name,
                    kind = i.BOOK.kind,
                    author = i.BOOK.author,
                    amount = i.amount,
                    inputId = i.INPUT.id
                }).ToListAsync();

            var listInput = await _context.INPUTS.Where(x => x.isRemove == false)
                .Select(i => new InputResponse
                {
                    id = i.id,
                    listInputInfo = detail.FindAll(x=> x.inputId == i.id),
                    isRemove = i.isRemove
                }).ToListAsync();

            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Data = listInput
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> Get(int id)
        {
            var exists = await _context.INPUTS.FindAsync(id);
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
                var detail = await _context.INPUTDETAILS
                .Include(i => i.INPUT).Include(b => b.BOOK)
                .Where(x => x.inputId == x.INPUT.id && x.INPUT.isRemove == false && x.bookId == x.BOOK.id)
                .Select(i => new InputDetailInfo
                {
                    stt = i.stt,
                    name = i.BOOK.name,
                    kind = i.BOOK.kind,
                    author = i.BOOK.author,
                    amount = i.amount,
                    inputId = i.INPUT.id
                }).ToListAsync();

                return new BaseResponse
                {
                    ErrorCode = Roles.Success,
                    Data = await _context.INPUTS.Where(x => x.isRemove == false && x.id == id)
                    .Select(i => new InputResponse
                    {
                        id = i.id,
                        listInputInfo = detail.FindAll(x => x.inputId == i.id),
                        isRemove = i.isRemove
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
        public async Task<ActionResult<BaseResponse>> Post([FromForm] InputResponse infoEnter)
        {
            // Thêm một phiếu nhập mới
            var input = new INPUT { isRemove = false };
            _context.INPUTS.Add(input);
            await _context.SaveChangesAsync();

            // Cập nhật phiếu nhập mới
            var list = infoEnter.listInputInfo;
            for (int i = 0; i < list.Count; i++)
            {
                // Kiểm tra dữ liệu nhập bị trống
                if(string.IsNullOrEmpty(list[i].name) ||
                    string.IsNullOrEmpty(list[i].kind) ||
                    string.IsNullOrEmpty(list[i].author) ||
                    list[i].amount == 0) {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Empty_Book_Input,
                        Message = "Some field is empty!",
                        Data = null
                    };
                }
                else
                {
                    // Sách này đã tồn tại trong kho
                    var book = await _context.BOOKS.Where(x => x.name == list[i].name).FirstOrDefaultAsync();
                    if(book != null && book.isRemove == false)
                    {
                        // check policy
                        Roles policy = new Roles();
                        if (book.stock > policy.MaxBookStock)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.OverflowMaxStock,
                                Message = "Maximum stock: " + policy.MaxBookStock + ". You cannot input!"
                            };
                        }
                        else if (book.stock <= policy.MinBookStock)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.NotEnoughMinStock,
                                Message = "Please input minimum " + policy.MinBookStock + " books!"
                            };
                        }
                        book.stock += list[i].amount;   // Cập nhật số lượng sách
                        book.kind = list[i].kind;
                        book.author = list[i].author;
                        _context.BOOKS.Update(book);
                        _context.INPUTDETAILS.Add(new INPUTDETAIL   // Thêm 1 inputDetail mới
                        {
                            stt = i + 1,
                            inputId = input.id,
                            bookId = book.id,
                            amount = list[i].amount
                        });
                        await _context.SaveChangesAsync();  // Lưu thay đổi
                    }
                    else
                    {
                        // Kho chưa có sách này
                        book = new BOOK();
                        book.name = list[i].name;
                        book.kind = list[i].kind;
                        book.author = list[i].author;
                        book.stock = list[i].amount;
                        book.price = 0;
                        book.state = true;
                        book.isRemove = false;
                        book.imageName = null;
                        book.url = null;
                        _context.BOOKS.Add(book);   // Thêm sách mới
                        _context.INPUTDETAILS.Add(new INPUTDETAIL   // Thêm 1 inputDetail mới
                        {
                            stt = i,
                            inputId = input.id,
                            bookId = book.id,
                            amount = list[i].amount
                        });
                        await _context.SaveChangesAsync();  // Lưu thay đổi
                    }
                }
            }
            
            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "A book input bill is created!"
            };
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Put([FromForm] InputResponse infoEnter)
        {
            // Cập nhật phiếu nhập mới
            var list = infoEnter.listInputInfo;
            for (int i = 0; i < list.Count; i++)
            {
                // Kiểm tra dữ liệu nhập bị trống
                if (string.IsNullOrEmpty(list[i].name) ||
                    string.IsNullOrEmpty(list[i].kind) ||
                    string.IsNullOrEmpty(list[i].author) ||
                    list[i].amount == 0)
                {
                    return new BaseResponse
                    {
                        ErrorCode = Roles.Empty_Book_Input,
                        Message = "Some field is empty!",
                        Data = null
                    };
                }
                else
                {
                    // Sách này đã tồn tại trong kho
                    var book = await _context.BOOKS.Where(x => x.name == list[i].name).FirstOrDefaultAsync();
                    if (book != null && book.isRemove == false)
                    {
                        // check policy
                        Roles policy = new Roles();
                        if (book.stock > policy.MaxBookStock)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.OverflowMaxStock,
                                Message = "Maximum stock: " + policy.MaxBookStock + ". You cannot input!"
                            };
                        }
                        else if (book.stock <= policy.MinBookStock)
                        {
                            return new BaseResponse
                            {
                                ErrorCode = Roles.NotEnoughMinStock,
                                Message = "Please input minimum " + policy.MinBookStock + " books!"
                            };
                        }
                        book.stock += list[i].amount;  
                        book.kind = list[i].kind;
                        book.author = list[i].author;
                        _context.BOOKS.Update(book);    // Cập nhật sách

                        var inputDetail = await _context.INPUTDETAILS
                            .Where(x => x.inputId == infoEnter.id && x.stt == list[i].stt).FirstOrDefaultAsync();
                        inputDetail.amount = list[i].amount;
                        _context.INPUTDETAILS.Update(inputDetail);    // Cập nhật inputDetail

                        await _context.SaveChangesAsync();  // Lưu thay đổi
                        return new BaseResponse
                        {
                            ErrorCode = Roles.Success,
                            Message = "Update successfully!"
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            ErrorCode = Roles.NotFound,
                            Message = "Not find this input!"
                        };
                    }
                }
            }

            return new BaseResponse
            {
                ErrorCode = Roles.Success,
                Message = "Book input bill is update!"
            };
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int id)
        {
            var exists = await _context.INPUTS.FindAsync(id);
            if (exists != null)
            {
                exists.isRemove = true;
                _context.INPUTS.Update(exists);
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