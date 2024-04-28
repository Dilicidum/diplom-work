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

        public DbSet<Criteria> Criterias { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<CandidateCriteria> CandidateCriterias { get; set; }
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

            modelBuilder.Entity<Tasks>()
                .HasMany(x=>x.Criterias)
                .WithOne(x=>x.Vacancy)
                .HasForeignKey(x=>x.VacancyId);

            modelBuilder.Entity<Tasks>()
                .HasMany(x=>x.Candidates)
                .WithOne(x=>x.Vacancy)
                .HasForeignKey(x=>x.VacancyId);

            modelBuilder.Entity<CandidateCriteria>()
        .HasKey(cc => new { cc.CandidateId, cc.CriteriaId });

    modelBuilder.Entity<CandidateCriteria>()
        .HasOne(cc => cc.Candidate)
        .WithMany(c => c.CandidateCriterias)
        .HasForeignKey(cc => cc.CandidateId)
        .OnDelete(DeleteBehavior.Restrict); // Restricting delete behavior

    modelBuilder.Entity<CandidateCriteria>()
        .HasOne(cc => cc.Criteria)
        .WithMany(c => c.CandidateCriterias)
        .HasForeignKey(cc => cc.CriteriaId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
