using Microsoft.EntityFrameworkCore;

namespace Foci.Models
{
    public class FociDbContext : DbContext
    {
        public FociDbContext(DbContextOptions<FociDbContext> options) : base(options)
        {

        }
        public DbSet<Meccs> Meccsek {  get; set; }
    }
}
