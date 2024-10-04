using Microsoft.EntityFrameworkCore;

namespace SignalRDenemesi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Person> People { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
