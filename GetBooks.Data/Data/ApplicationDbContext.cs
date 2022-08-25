using Microsoft.EntityFrameworkCore;
using GetBooks.Models;

namespace GetBooks.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
    }
}