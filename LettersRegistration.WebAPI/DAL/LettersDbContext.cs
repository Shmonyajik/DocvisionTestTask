using LettersRegistration.WebAPI.Domain;

namespace LettersRegistration.WebAPI.DAL
{
    public class LettersDbContext : DbContext
    {
        public LettersDbContext(DbContextOptions<LettersDbContext> options) : base(options) { }
        public DbSet<Letter> Letters { get; set; }
    }
}

