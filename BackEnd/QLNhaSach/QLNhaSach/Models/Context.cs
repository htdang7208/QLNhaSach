using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    public class Context :DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
        public virtual DbSet<ADMIN> ADMINS { get; set; }
        public virtual DbSet<CUSTOMER> CUSTOMERS { get; set; }
        public virtual DbSet<BOOK> BOOKS { get; set; }
        public virtual DbSet<INPUT> INPUTS { get; set; }
        //public virtual DbSet<INPUTDETAIL> INPUTDETAILS { get; set; }
        public virtual DbSet<RECEIPT> RECEIPTS { get; set; }
        public virtual DbSet<SALE> SALES { get; set; }
        public virtual DbSet<SALEDETAIL> SALEDETAILS { get; set; }
    }
}