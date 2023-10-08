namespace DAL
{
    using DAL.Models;
    using Microsoft.EntityFrameworkCore;    
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            modelBuilder.Entity<User>().HasKey(x => x.Id);
        }
    }
}