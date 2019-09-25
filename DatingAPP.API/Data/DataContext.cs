using Microsoft.EntityFrameworkCore;
using DatingAPP.API.Models;
namespace DatingAPP.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options){}
        public DbSet<Value> Values {get;set;}
    }
}