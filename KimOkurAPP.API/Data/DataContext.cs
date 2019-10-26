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
        public DbSet<Like> Likes { get; set; }

   protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.Entity<Like>()
                .HasKey(k => new {k.LikerId, k.LikeeId});

            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}