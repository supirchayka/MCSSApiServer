using MCSSApiServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCSSApiServer
{
    public class DatabaseContext : DbContext
    { 
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
