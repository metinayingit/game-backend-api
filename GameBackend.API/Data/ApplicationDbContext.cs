using GameBackend.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
    }
}