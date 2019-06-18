using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLNhaSach.Models.Response;
using QLNhaSach.Models.Responses;
using QLNhaSach.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QLNhaSach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeRolesController : Controller
    {
        [HttpGet]
        public RoleInfo Get()
        {
            RoleInfo role = new RoleInfo();
            role.MaxBookStock = Roles.MaxBookStock;
            role.MinBookInput = Roles.MinBookInput;
            role.GetOverDept = Roles.GetOverDept;
            role.DeptOver = Roles.DeptOver;
            role.StockMax = Roles.StockMax;
            return role;
        }
        [HttpPost]
        public bool Post(RoleInfo roleInfo)
        {
            Roles.MaxBookStock = roleInfo.MaxBookStock;
            Roles.MinBookInput = roleInfo.MinBookInput;
            Roles.GetOverDept = roleInfo.GetOverDept;
            Roles.DeptOver = roleInfo.DeptOver;
            Roles.StockMax = roleInfo.StockMax;
            return true;
        }
    }
}
