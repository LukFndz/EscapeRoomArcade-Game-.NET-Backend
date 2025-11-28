using Microsoft.EntityFrameworkCore;
using EscapeRoomApi.Models;

namespace EscapeRoomApi.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<UserProfile> Users { get; set; }
    }
}
