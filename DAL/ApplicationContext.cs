namespace DAL
{
    using DAL.Models;
    using Microsoft.EntityFrameworkCore;    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class ApplicationContext: IdentityDbContext<IdentityUser,IdentityRole,string>
    {
        public DbSet<Tasks> Tasks { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tasks>()
            .HasMany(t => t.SubTasks)
            .WithOne()
            .HasForeignKey(t => t.BaseTaskId);

             modelBuilder.Entity<Tasks>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId); 
        }
    }
}