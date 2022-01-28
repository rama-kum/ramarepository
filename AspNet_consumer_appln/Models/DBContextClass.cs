using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNet_consumer_appln.Models
{
    public class DBcontextClass : DbContext
    {
        public DBcontextClass(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BillingClass> patient_Table { get; set; }
    }
}
