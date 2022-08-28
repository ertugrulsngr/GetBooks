using Microsoft.EntityFrameworkCore;
using GetBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GetBooks.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
    }
}