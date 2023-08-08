using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.DB
{
    public class TravelContext : DbContext
    {
        public DbSet<Images> Images { get; set; }

        public TravelContext(DbContextOptions<TravelContext> options) : base(options)
        {

        }
    }
}
