namespace DAL
{
    using DAL.Models;
    using Microsoft.EntityFrameworkCore;    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class ApplicationContext: IdentityDbContext<IdentityUser,IdentityRole,string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "User"},
                new IdentityRole() { Name = "Admin" });
        }
    }
}