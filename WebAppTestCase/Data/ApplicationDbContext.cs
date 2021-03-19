using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTestCase.Models;

namespace WebAppTestCase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Appeal> Appeals { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
    }
}
