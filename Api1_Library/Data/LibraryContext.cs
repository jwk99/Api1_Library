using Api1_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Api1_Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();
            modelBuilder.Entity<Book>()
                .HasCheckConstraint("CK_Books_Copies", "Copies >= 0");
        }
    }
}
