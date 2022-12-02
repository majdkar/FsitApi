using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskEmployees.Model;

namespace TaskEmployees.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Category> TblCategory { get; set; }

        public DbSet<Product> TblProduct { get; set; }

        public DbSet<Users> TblUsers { get; set; }
    }
}
