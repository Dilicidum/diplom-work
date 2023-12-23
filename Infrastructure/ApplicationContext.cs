using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure
{
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
