using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.DB
{
    public class ImageContext : DbContext
    {
      
        public DbSet<Images> Images { get; set; }
  

        public ImageContext(DbContextOptions<ImageContext> options) : base(options)
        {

        }
    }
}
