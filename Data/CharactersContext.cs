using Microsoft.EntityFrameworkCore;

namespace RPG.Data
{
    public class CharactersContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public CharactersContext(DbContextOptions<CharactersContext> options) : base(options)
        {

        }
    }
}
