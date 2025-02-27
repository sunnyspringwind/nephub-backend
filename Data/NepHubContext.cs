using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NepHubAPI.Models;

namespace NepHubAPI.Data;
public class NepHubContext(DbContextOptions<NepHubContext> options) : IdentityDbContext<AppUser>(options)
{
    //Ranking Library Dbset
    public DbSet<Entity> Entities { get; set; }

    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<AttributeEntry> AttributeEntires { get; set; }
    public DbSet<Quiziz> Quiziz { get; set; }
    public DbSet<Timeline> Timeline { get; set; }

    public DbSet<QuizScore> QuizScores { get; set; }

    public DbSet<UpdateRequest> UpdateRequests { get; set; }


    // explicit configuration in OnModelCreating allows you to fine-tune and control behaviors like cascade delete, foreign key names, and how relationships should be handled when entities are deleted or updated

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); //configures internal identity context

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );

        modelBuilder.Entity<QuizScore>()
            .HasOne(qs => qs.User)
            .WithMany(u => u.QuizScores)
            .HasForeignKey(qs => qs.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Entity)
            .WithMany(e => e.Reactions)
            .HasForeignKey(r => r.EntityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AttributeEntry>()
            .HasOne(a => a.Entity)
            .WithMany(e => e.Attributes)
            .HasForeignKey(a => a.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}