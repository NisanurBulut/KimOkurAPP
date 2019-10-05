using KimOkur.API.Models;
using KimOkur.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KimOkur.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base (options){}
        public DbSet<Value> Values {get;set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}