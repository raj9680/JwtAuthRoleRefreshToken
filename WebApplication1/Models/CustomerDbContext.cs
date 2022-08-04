using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Models
{
    public class CustomerDbContext: DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            :base(options)
        {
            
        }

        // Registering tables
        public DbSet<TblCustomer> TblCustomerr { get; set; }
        public DbSet<TblUser> TblUserr { get; set; }
        public DbSet<TblRefreshtoken> TblRefreshtokenn { get; set; }

        /// Static Connection To Database
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server = .; Database = Transmission; Integrated Security = True");
        //     base.OnConfiguring(optionsBuilder);
        // }
    }
}
