using Microsoft.EntityFrameworkCore;
using UserManagementApi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UserManagementApi.Repositories
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        //This represents table in the database
        public DbSet<User> Users { get; set; }
    }
}
