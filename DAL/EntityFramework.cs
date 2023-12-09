
using Feed;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL
{
    public class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=hakaton;Username=postgres;Password=1234");
        }
        public DbSet<FeedBack> feedBacks { get; set; }

    }

}
