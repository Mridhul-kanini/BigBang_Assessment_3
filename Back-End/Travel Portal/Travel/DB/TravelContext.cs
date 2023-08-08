using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.DB
{
    public class TravelContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Bookingdetails> Bookingdetails { get; set; }
        public DbSet<Carddetails> Carddetails { get; set; }
        public DbSet<Hotels> Hotels { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<TravelAgent> TravelAgents { get; set; }
        public DbSet<User> Users { get; set; }

        public TravelContext(DbContextOptions<TravelContext> options) : base(options)
        {

        }
    }
}
