using Micro_House_Manage_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_House_Manage_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }

        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Listing> Listings { get; set; }
    }
}
